using EventService.API.Application.Queries.BrandQueries;
using MediatR;

namespace EventService.API.Controllers {
    public class BrandServices(
    IMediator mediator,
    IBrandQueries queries,
    ILogger<BrandServices> logger) {
        public IMediator Mediator { get; set; } = mediator;
        public ILogger<BrandServices> Logger { get; } = logger;
        public IBrandQueries Queries { get; } = queries;
    }
}
