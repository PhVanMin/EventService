using MediatR;

namespace EventService.API.Application.Commands.BrandCommands {
    public record UpdateBrandCommand(
        int id,
        string Name,
        string Field,
        short Status
    ) : IRequest<bool>;
}
