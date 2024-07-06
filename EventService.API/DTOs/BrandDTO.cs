namespace EventService.API.DTOs;
public record BrandDTO
(
    long Id,
    string Name,
    string Field,
    string Address,
    string? Gps,
    short Status
);
