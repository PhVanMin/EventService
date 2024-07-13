namespace EventService.API.DTOs.Event;
public record CreateEventRequest
(
    string Name,
    string Image,
    int NoVoucher,
    DateTime StartDate,
    DateTime EndDate,
    int BrandId,
    int? GameId
);
