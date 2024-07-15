using EventService.API.Application.Queries.BrandQueries;
using MediatR;

namespace EventService.API.Controllers {
    public class ControllerServices(
    IMediator mediator,
    IBrandQueries queries,
    ILogger<ControllerServices> logger) {
        public IMediator Mediator { get; set; } = mediator;
        public ILogger<ControllerServices> Logger { get; } = logger;
        public IBrandQueries Queries { get; } = queries;
    }
}
