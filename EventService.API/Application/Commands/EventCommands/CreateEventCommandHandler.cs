using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var brand = await _context.Brands
                .Include(b => b.Events)
                .FirstOrDefaultAsync(b => b.Id == request.brandId);

            if (brand == null)
                return false;

            if (brand.Events.FirstOrDefault(e => e.Name == request.name) != null)
                return false;

            brand.AddEvent(request.name, request.image, request.noVoucher, request.start, request.end, request.gameId, request.voucherIds);
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
