using EventService.Infrastructure.Idempotency;
using EventService.Infrastructure;
using MediatR;

namespace EventService.API.Application.Commands.BrandCommands {
    public class DeleteBrandCommandHanlder : IRequestHandler<DeleteBrandCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<DeleteBrandCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public DeleteBrandCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<DeleteBrandCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken) {
            var brand = await _context.Brands.FindAsync(request.id);
            if (brand == null) {
                return false;
            }

            _logger.LogInformation("Deleting Brand - Brand: {@Brand}", brand);
            _context.Brands.Remove(brand);
            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class DeleteBrandIdentifiedCommandHanlder : IdentifiedCommandHandler<DeleteBrandCommand, bool> {
        public DeleteBrandIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<DeleteBrandCommand, bool>> logger)
        : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
