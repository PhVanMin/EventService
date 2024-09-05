using EventService.API.Application.IntegrationEvents.Message;
using EventService.API.Application.IntegrationEvents;
using Quartz;

namespace EventService.API.Application.ScheduleJob {
    public class NotifyEventEnd : IJob {
        private readonly IntegrationEventService _integrationEventService;
        private readonly ILogger<NotifyEventEnd> _logger;

        public NotifyEventEnd(ILogger<NotifyEventEnd> logger, IntegrationEventService integrationEventService) {
            _integrationEventService = integrationEventService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context) {
            var data = context.JobDetail.JobDataMap.GetString("message");
            if (data == null) {
                throw new Exception("Data is null.");
            }

            var values = data.Split("|");
            if (values.Length != 2) {
                throw new Exception("Data is length is 2.");
            }

            _logger.LogInformation("Publishing {type} to Game {id}", typeof(NotifyEventEnd), values[0]);
            await _integrationEventService.PublishIntegrationEvent(new EndGameMessage {
                eventId = int.Parse(values[0]),
                gameId = Guid.Parse(values[1])
            }, context.CancellationToken);
        }
    }
}
