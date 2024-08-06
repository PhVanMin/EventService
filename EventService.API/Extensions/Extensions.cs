using EventService.API.Application.Behaviors;
using EventService.API.Application.Queries;
using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Extensions
{
    internal static class Extensions {
        public static void AddApplicationServices(this IHostApplicationBuilder builder) {
            builder.Services.AddMediatR(cfg =>
            {
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
        }

        public static void AddLoggingServices(this IHostApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
        }
    }
}
