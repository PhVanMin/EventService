using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record CreateVoucherCommand(
        int brandId,
        string Image,
        int Value,
        string Description,
        int ExpireDate,
        int Status
    ) : IRequest<bool>;
}
