using EventService.Infrastructure.Idempotency;
using EventService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Commands.EventCommands
{
    public class UpdateEventCommandHanlder : IRequestHandler<UpdateEventCommand, bool>
    {
        private readonly EventContext _context;
        private readonly ILogger<UpdateEventCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public UpdateEventCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<UpdateEventCommandHanlder> logger)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(
                e => e.Id == request.id
                && e.BrandId == request.brandId);
            if (@event == null)
            {
                return false;
            }

            _logger.LogInformation("Updating Brand Event - Brand Event: {@Event}", @event);
            @event.Update(
                request.name, request.image,
                request.noVoucher, request.start,
                request.end, request.gameId);

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class UpdateEventIdentifiedCommandHanlder : IdentifiedCommandHandler<UpdateEventCommand, bool>
    {
        public UpdateEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateEventCommand, bool>> logger)
        : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
