using MediatR;

namespace EventService.API.Application.Commands.EventCommands {
    public record AddRedeemVoucherOfEventCommand(int eventId, int voucherId) : IRequest<bool>;
}
