using MediatR;

namespace EventService.Domain.Events {
    public record EventVoucherUsedUpDomainEvent(
        int eventId
    ) : INotification;
}
