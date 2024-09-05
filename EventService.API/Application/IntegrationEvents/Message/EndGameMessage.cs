using BuildingBlocks.Messaging.Events;

namespace EventService.API.Application.IntegrationEvents.Message {
    public record EndGameMessage : IntegrationEvent {
        public int eventId { get; set; }
        public Guid gameId { get; set; }
    };
}
