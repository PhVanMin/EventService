using MediatR;

namespace EventService.API.Application.Commands.EventCommands {
    public record AddPlayerToEventCommand(
        int eventId, string name, string email
    ) : IRequest<bool>;
}
