using EventService.API.Application.Commands.EventCommands;
using EventService.API.Application.IntegrationEvents.Message;
using MassTransit;
using MediatR;

namespace EventService.API.Application.IntegrationEvents.Consumers {
    public class PlayerJoinEventConsumer : IConsumer<PlayerJoinEventMessage> {
        private readonly ILogger<PlayerJoinEventConsumer> _logger;
        private readonly IMediator _mediator;
        public PlayerJoinEventConsumer(IMediator mediator,
            ILogger<PlayerJoinEventConsumer> logger) {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PlayerJoinEventMessage> context) {
            var message = context.Message;
            var command = new AddPlayerToEventCommand(message.eventId, message.name, message.email);

            _logger.LogInformation("Hanling Integration Event {type} - {@Event}", nameof(PlayerJoinEventMessage), message);
            await _mediator.Send(command);
        }
    }
}
