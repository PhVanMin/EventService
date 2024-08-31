using EventService.Domain.AggregateModels.EventAggregate;
using MediatR;

namespace EventService.Domain.Events {
    public record EventGameRegisteredOrUpdateDomainEvent(
        Event @event, int gameId    
    ) : INotification;
}
