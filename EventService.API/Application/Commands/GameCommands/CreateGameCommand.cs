using MediatR;

namespace EventService.API.Application.Commands.GameCommands {
    public record CreateGameCommand(
        string Name,
        string Image
    ) : IRequest<bool>;
}
