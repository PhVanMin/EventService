using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record CreateVoucherCommand(
        int brandId,
        IFormFile Image,
        string Code,
        int Value,
        string? Description,
        int ExpireDate,
        int Status
    ) : IRequest<bool>;
}
