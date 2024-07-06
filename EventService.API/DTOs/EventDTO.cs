namespace EventService.API.DTOs;
public record EventDTO
(
    long Id,
    string Name,
    string Image,
    long NoVoucher,
    DateOnly StartDate,
    DateOnly EndDate,
    long BrandId,
    long? GameId
);
