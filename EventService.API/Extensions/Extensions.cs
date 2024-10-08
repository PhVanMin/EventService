﻿using Azure.Identity;
using BuildingBlocks.Messaging.MassTransit;
using EventService.API.Application.Behaviors;
using EventService.API.Application.IntegrationEvents;
using EventService.API.Application.Queries;
using EventService.API.Application.ScheduleJob;
using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Simpl;
using System.Text;

namespace EventService.API.Extensions {
    internal static class Extensions {
        public static void AddApplicationServices(this IHostApplicationBuilder builder) {
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                //cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            builder.Services.AddScoped<IRequestManager, RequestManager>();
            builder.Services.AddScoped<IEventQueries, EventQueries>();
            builder.Services.AddScoped<EventAPIService>();
            builder.Services.AddScoped<AzureClientService>();
            builder.Services.AddScoped<IntegrationEventService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<EventDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("database")
            ));

            builder.Services.AddGrpc();
            builder.Services.AddSwaggerGen(c => {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            builder.Services.AddQuartz(q => {
                q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
            });
            builder.Services.AddQuartzHostedService();

            builder.Services.AddAzureClients(clientBuilder => {
                clientBuilder.AddBlobServiceClient(new Uri(builder.Configuration["AzureStorage:Uri"]!));
                clientBuilder.UseCredential(new DefaultAzureCredential());
            });

            builder.Services.AddMessageBroker(builder.Configuration, typeof(Program).Assembly);
        }

        public static void ConfigurateLogging(this IHostApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
        }

        public static void AddAuthenticationServices(this IHostApplicationBuilder builder) {
            var signingKey = builder.Configuration["JWT:SigningKey"];
            if (string.IsNullOrEmpty(signingKey)) {
                throw new InvalidOperationException("JWT SigningKey is not configured.");
            }

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                };
            });
        }
    }
}
