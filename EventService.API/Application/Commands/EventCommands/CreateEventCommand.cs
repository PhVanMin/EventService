using MediatR;

namespace EventService.API.Application.Commands.EventCommands {
    public record CreateEventCommand(
        int brandId,
        string name,
        string image,
        int noVoucher,
        DateTime start,
        DateTime end,
        int gameId,
        List<VoucherDTO> vouchers
    ) : IRequest<bool>;

    public record VoucherDTO {
        public string Code = null!;
        public string Image = null!;
        public int Value;
        public string Description = null!;
        public DateTime ExpireDate;
        public int Status;
    }
}
