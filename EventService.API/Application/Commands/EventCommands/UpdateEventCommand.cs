using MediatR;

namespace EventService.API.Application.Commands.EventCommands
{
    public record UpdateEventCommand(
        int brandId,
        int id,
        string name,
        string image,
        int noVoucher,
        DateTime start,
        DateTime end,
        int gameId,
        List<int> voucherIds
    ) : IRequest<bool>;
}
