using MediatR;

namespace EventService.API.Application.Commands.EventCommands
{
    public record UpdateEventCommand(
        int brandId,
        int id,
        string? name,
        int? noVoucher,
        DateTime? start,
        DateTime? end,
        Guid? gameId,
        IFormFile? image,
        List<int>? voucherIds
    ) : IRequest<bool>;
}
