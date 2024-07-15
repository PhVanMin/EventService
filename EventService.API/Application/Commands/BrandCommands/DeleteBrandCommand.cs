using MediatR;

namespace EventService.API.Application.Commands.BrandCommands {
    public record DeleteBrandCommand(int id) : IRequest<bool>;
}
