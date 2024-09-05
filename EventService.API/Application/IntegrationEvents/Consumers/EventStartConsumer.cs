using EventService.API.Application.IntegrationEvents.Message;
using MassTransit;

namespace EventService.API.Application.IntegrationEvents.Consumers {
    public class EventStartConsumer : IConsumer<StartGameMessage> {
        ILogger<EventStartConsumer> _logger;
        public EventStartConsumer(ILogger<EventStartConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<StartGameMessage> context) {
            _logger.LogInformation("Event {id} start", context.Message.eventId);
            return Task.CompletedTask;
        }
    }
}
