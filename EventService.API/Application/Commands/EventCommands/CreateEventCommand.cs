using MediatR;

namespace EventService.API.Application.Commands.EventCommands {
    public record CreateEventCommand(
        int brandId,
        string name,
        IFormFile image,
        int noVoucher,
        DateTime start,
        DateTime end,
        Guid? gameId,
        List<int>? voucherIds
    ) : IRequest<bool>;
}
