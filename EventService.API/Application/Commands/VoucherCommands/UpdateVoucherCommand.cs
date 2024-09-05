using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public record UpdateVoucherCommand(
        int id,
        IFormFile? image,
        string? code,
        string? description,
        int? value,
        int? expireDate,
        int? status
    ) : IRequest<bool>;
}
