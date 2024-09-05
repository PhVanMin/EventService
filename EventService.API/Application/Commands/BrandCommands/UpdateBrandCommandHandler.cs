using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.BrandCommands {
    public class UpdateBrandCommandHanlder : IRequestHandler<UpdateBrandCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<UpdateBrandCommandHanlder> _logger;
        public UpdateBrandCommandHanlder(
            EventDbContext context,
            ILogger<UpdateBrandCommandHanlder> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateBrandCommand request, CancellationToken cancellationToken) {
            var brand = await _context.Brands.FindAsync(request.id);
            if (brand == null) {
                return false;
            }

            _logger.LogInformation("Updating Brand - Brand: {@Brand}", brand);
            brand.Update(request.Name, request.Field, request.Status);
            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class UpdateBrandIdentifiedCommandHanlder : IdentifiedCommandHandler<UpdateBrandCommand, bool> {
        public UpdateBrandIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateBrandCommand, bool>> logger)
        : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return true;
        }
    }
}
