using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record AddVoucherToEventCommand(
        int voucherId,
        int eventId
    ) : IRequest<bool>;
}
