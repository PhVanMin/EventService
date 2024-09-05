using BuildingBlocks.Messaging.Events;

namespace EventService.API.Application.IntegrationEvents.Message {
    public record PlayerJoinEventMessage(int eventId, string name, string email) : IntegrationEvent;
}
