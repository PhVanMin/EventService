using EventService.API.Application.Behaviors;
using EventService.API.Application.Queries;
using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<EventContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("database")
            ));

            builder.Services.AddGrpc();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }

        public static void AddLoggingServices(this IHostApplicationBuilder builder) {
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
