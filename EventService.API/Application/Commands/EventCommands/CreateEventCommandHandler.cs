using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.EventCommands
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, bool>
    {
        private readonly EventContext _context;
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly IMediator _mediator;
        public CreateEventCommandHandler(IMediator mediator,
            EventContext context,
            ILogger<CreateEventCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var brand = await _context.Brands.FindAsync(request.brandId);
            if (brand == null)
                return false;

            brand.AddEvent(request.name, request.image, request.noVoucher, request.start, request.end, request.gameId);
            foreach(var voucher in request.vouchers) {
                brand.AddVoucher(voucher.Code, voucher.Image, voucher.Value, voucher.Description, voucher.ExpireDate, voucher.Status);
            }
            _logger.LogInformation("Adding Event to Brand - Event: {@Event}", brand.Events.Last());

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class CreateEventIdentifiedCommandHanlder : IdentifiedCommandHandler<CreateEventCommand, bool>
    {
        public CreateEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateEventCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return false;
        }
    }
}
