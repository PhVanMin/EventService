using MediatR;

namespace EventService.API.Application.Commands.EventCommands {
    public record AddRedeemVoucherOfEventCommand(int eventId) : IRequest<RedeemVoucherDataVM?>;
    public record RedeemVoucherDataVM(string code, int voucherId, int expireDate);
}
