using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record UpdateVoucherCommand(
        int id,
        string image,
        string description,
        int value,
        int expireDate,
        int status
    ) : IRequest<bool>;
}
