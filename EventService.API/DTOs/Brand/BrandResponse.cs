namespace EventService.API.DTOs.Brand {
    public record BrandResponse(
        string Name,
        string Field,
        string? Address,
        string? Gps,
        short Status
    );
}
