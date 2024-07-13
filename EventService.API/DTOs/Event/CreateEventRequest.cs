namespace EventService.API.DTOs.Event;
public record CreateEventRequest
(
    string Name,
    string Image,
    long NoVoucher,
    DateTime StartDate,
    DateTime EndDate,
    long BrandId,
    long? GameId
);
