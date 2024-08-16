using EventService.API.Application.Queries;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record CreateRedeemVoucherCommand(
        int eventId,
        int playerId,
        int voucherId
    ) : IRequest<RedeemVoucherVM?>;
}
