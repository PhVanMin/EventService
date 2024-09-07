using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.BrandCommands {
    public class CreateBrandCommandHanlder : IRequestHandler<CreateBrandCommand, int> {
        private readonly EventDbContext _context;
        private readonly ILogger<CreateBrandCommandHanlder> _logger;
        public CreateBrandCommandHanlder(
            EventDbContext context,
            ILogger<CreateBrandCommandHanlder> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Handle(CreateBrandCommand request, CancellationToken cancellationToken) {
            var location = new Location(request.Address, request.Gps);
            var brand = new Brand() {
                Name = request.Name,
                Location = location,
                Field = request.Field,
                Status = request.Status
            };

            _logger.LogInformation("Creating Brand - Brand: {@Brand}", brand);
            _context.Brands.Add(brand);

            var result = await _context.SaveEntitiesAsync(cancellationToken);
            return result ? brand.Id : -1;
        }
    }

    public class CreateBrandIdentifiedCommandHanlder : IdentifiedCommandHandler<CreateBrandCommand, int> {
        public CreateBrandIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateBrandCommand, int>> logger)
        : base(mediator, requestManager, logger) { }

        protected override int CreateResultForDuplicateRequest() {
            return -1;
        }
    }
}
