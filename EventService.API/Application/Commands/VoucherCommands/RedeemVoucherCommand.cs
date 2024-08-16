using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record RedeemVoucherCommand(
        int eventId,
        int redeemVoucherId,
        string code
    ) : IRequest<bool>;
}
