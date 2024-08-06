using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record UpdateEventVouchersCommand(
        List<int> voucherIds,
        int eventId
    ) : IRequest<bool>;
}
