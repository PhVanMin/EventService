using EventService.API.Application.Queries;
using MediatR;

namespace EventService.API.Controllers
{
    public class EventAPIService(
    IMediator mediator,
    IEventQueries queries,
    ILogger<EventAPIService> logger) {
        public IMediator Mediator { get; set; } = mediator;
        public ILogger<EventAPIService> Logger { get; } = logger;
        public IEventQueries Queries { get; } = queries;
    }
}
