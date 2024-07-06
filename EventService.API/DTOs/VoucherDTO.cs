namespace EventService.API.DTOs;
public record VoucherDTO
(
    long Id,
    string Code,
    string Image,
    long Value,
    string Description,
    DateOnly ExpireDate,
    short Status,
    long EventId
);
