using MediatR;

namespace EventService.API.Application.Commands.BrandCommands {
    public record CreateBrandCommand(
        string Name,
        string Field,
        string? Address,
        string? Gps,
        int Status
    ) : IRequest<bool>;
}
