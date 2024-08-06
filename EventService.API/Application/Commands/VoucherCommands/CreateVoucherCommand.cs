using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record CreateVoucherCommand(
        int brandId,
        string Code,
        string Image,
        int Value,
        string Description,
        DateTime ExpireDate,
        int Status
    ) : IRequest<bool>;
}
