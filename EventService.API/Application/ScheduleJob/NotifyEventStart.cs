﻿using EventService.API.Application.IntegrationEvents;
using EventService.API.Application.IntegrationEvents.Message;
using MassTransit;
using Quartz;

namespace EventService.API.Application.ScheduleJob {
    public class NotifyEventStart : IJob {
        private readonly IntegrationEventService _integrationEventService;
        private readonly ILogger<NotifyEventStart> _logger;
        private readonly IntegrationEventService _service;

        public NotifyEventStart(ILogger<NotifyEventStart> logger, IntegrationEventService integrationEventService) {
            _integrationEventService = integrationEventService;
            _logger = logger;
            _service = integrationEventService;
        }

        public async Task Execute(IJobExecutionContext context) {
            var data = context.JobDetail.JobDataMap.GetString("message");
            if (data == null) {
                throw new Exception("Data is null.");
            }

            var values = data.Split("|");
            if (values.Length != 2) {
                throw new Exception("Data length must be 2.");
            }

            _logger.LogInformation("Publishing {type} to Game {id}", typeof(StartGameMessage), values[0]);
            await _integrationEventService.PublishIntegrationEvent(new StartGameMessage {
                eventId = int.Parse(values[0]),
                gameId = Guid.Parse(values[1])
            }, context.CancellationToken);
        }
    }
}
