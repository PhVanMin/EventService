using EventService.API.Application.ScheduleJob;
using EventService.Domain.Events;
using MediatR;
using Quartz;

namespace EventService.API.Application.DomainEventHandlers {
    public class EventVoucherUsedUpOrEndDomainEventHandler : INotificationHandler<EventVoucherUsedUpOrEndDomainEvent> {
        private ISchedulerFactory _schedulerFactory;
        private ILogger<EventVoucherUsedUpOrEndDomainEventHandler> _logger;

        public EventVoucherUsedUpOrEndDomainEventHandler(ISchedulerFactory schedulerFactory, ILogger<EventVoucherUsedUpOrEndDomainEventHandler> logger) {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }

        public async Task Handle(EventVoucherUsedUpOrEndDomainEvent notification, CancellationToken cancellationToken) {
            var jobKeyString = nameof(NotifyEventEnd) + notification.@event.Id;
            var jobKey = new JobKey(jobKeyString);
            var scheduler = await _schedulerFactory.GetScheduler();

            await scheduler.DeleteJob(jobKey, cancellationToken);

            if (notification.@event.GameId == null)
                throw new Exception("Event does not register a game.");

            var job = JobBuilder
                .Create<NotifyEventEnd>()
                .WithIdentity(jobKeyString, "DynamicEndGroup")
                .UsingJobData("message", $"{notification.@event.Id}|{notification.@event.GameId.Value}")
                .Build();

            DateTime endTime = notification.@event.EndDate < DateTime.UtcNow ? DateTime.UtcNow.AddSeconds(10) : notification.@event.EndDate;
            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"Trigger-{jobKeyString}", "DynamicEndGroup")
                .StartAt(endTime)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            _logger.LogInformation("Handling Domain Event: {event}", nameof(EventVoucherUsedUpOrEndDomainEvent));
        }
    }
}
