namespace EventService.API.DTOs.Brand;
public record CreateBrandRequest
(
    string Name,
    string Field,
    string Address,
    string? Gps,
    short Status
);
