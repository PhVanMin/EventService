using EventService.Domain.Events;
using EventService.Infrastructure;
using MediatR;

namespace EventService.API.Application.DomainEventHandlers {
    public class EventGameRegisterOrUpdateDomainEventHandler : INotificationHandler<EventGameRegisteredOrUpdateDomainEvent> {
        private EventDbContext _context;
        private ILogger<EventStartDomainEventHandler> _logger;

        public EventGameRegisterOrUpdateDomainEventHandler(EventDbContext context, ILogger<EventStartDomainEventHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(EventGameRegisteredOrUpdateDomainEvent notification, CancellationToken cancellationToken) {
            _logger.LogInformation("Handling Domain Event: {@event}", nameof(EventStartDomainEvent));

            if (notification.@event.IsTransient()) {
                await _context.SaveEntitiesAsync(cancellationToken);
            }

            _logger.LogInformation("EventGameRegisterOrUpdateDomainEventHandler Event ID - Game ID: {$ID} -{$GameId}", notification.@event.Id, notification.gameId);
        }
    }
}
