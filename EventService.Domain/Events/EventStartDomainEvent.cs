using EventService.Domain.AggregateModels.EventAggregate;
using MediatR;

namespace EventService.Domain.Events {
    public record EventStartDomainEvent(
        Event @event
    ) : INotification;
}
