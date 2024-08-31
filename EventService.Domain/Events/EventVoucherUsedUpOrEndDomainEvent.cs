using MediatR;

namespace EventService.Domain.Events {
    public record EventVoucherUsedUpOrEndDomainEvent(
        int eventId
    ) : INotification;
}
