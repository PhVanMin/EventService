using MediatR;

namespace EventService.API.Application.Commands.EventCommands {
    public record CreateEventCommand(
        int brandId,
        string name,
        string image,
        int noVoucher,
        DateTime start,
        DateTime end,
        int? gameId,
        List<int> voucherIds
    ) : IRequest<bool>;
}
