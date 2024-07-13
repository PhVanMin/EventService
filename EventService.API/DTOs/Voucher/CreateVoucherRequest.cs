namespace EventService.API.DTOs.Voucher;
public record CreateVoucherRequest
(
    string Code,
    string Image,
    long Value,
    string Description,
    DateTime ExpireDate,
    short Status,
    long EventId
);
