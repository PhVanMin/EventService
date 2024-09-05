using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Commands.EventCommands {
    public class AddPlayerToEventCommandHandler : IRequestHandler<AddPlayerToEventCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<AddPlayerToEventCommandHandler> _logger;
        public AddPlayerToEventCommandHandler(
            EventDbContext context,
            ILogger<AddPlayerToEventCommandHandler> logger) {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> Handle(AddPlayerToEventCommand request, CancellationToken cancellationToken) {
            var ev = await _context.Events.Include(e => e.Players).FirstOrDefaultAsync(e => e.Id == request.eventId);
            if (ev == null) {
                return false;
            }

            _logger.LogInformation("Adding Player to Event - Event: {EventId}", ev.Id);
            ev.AddPlayer(request.name, request.email);
            return await _context.SaveEntitiesAsync();
        }
    }

    public class AddPlayerToEventIdentifiedCommandHanlder : IdentifiedCommandHandler<AddPlayerToEventCommand, bool> {
        public AddPlayerToEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<AddPlayerToEventCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
