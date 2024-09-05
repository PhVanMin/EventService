using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace EventService.API.Application.IntegrationEvents {
    public class IntegrationEventService {
        private readonly IBus _bus;
        private readonly ILogger<IntegrationEventService> _logger;
        public IntegrationEventService(
            ILogger<IntegrationEventService> logger,
            IBus bus) {
            _bus = bus;
            _logger = logger;
        }

        public async Task PublishIntegrationEvent<T>(T message, CancellationToken token) where T : IntegrationEvent {
            _logger.LogInformation("Sending Integration Event Message - {@message}", message);
            await _bus.Publish(message, token);
        }
    }
}
