using EventService.API.Application.ScheduleJob;
using EventService.Domain.Events;
using MediatR;
using Quartz;

namespace EventService.API.Application.DomainEventHandlers {
    public class EventStartDomainEventHandler : INotificationHandler<EventStartDomainEvent> {
        private ISchedulerFactory _scheduler;
        private ILogger<EventStartDomainEventHandler> _logger;

        public EventStartDomainEventHandler(ISchedulerFactory scheduler, ILogger<EventStartDomainEventHandler> logger) {
            _logger = logger;
            _scheduler = scheduler;
        }

        public async Task Handle(EventStartDomainEvent notification, CancellationToken cancellationToken) {
            var jobKeyString = nameof(NotifyEventStart) + notification.@event.Id;
            var jobKey = new JobKey(jobKeyString);
            var scheduler = await _scheduler.GetScheduler();

            await scheduler.DeleteJob(jobKey, cancellationToken);

            if (notification.@event.GameId == null)
                throw new Exception("Event does not register a game.");

            var job = JobBuilder
                .Create<NotifyEventStart>()
                .WithIdentity(jobKeyString, "DynamicStartGroup")
                .UsingJobData("message", $"{notification.@event.Id}|{notification.@event.GameId.Value}")
                .Build();

            DateTime startTime = notification.@event.StartDate > DateTime.UtcNow ? DateTime.UtcNow.AddSeconds(10) : notification.@event.StartDate;
            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"Trigger-{jobKeyString}", "DynamicStartGroup")
                .StartAt(startTime)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            _logger.LogInformation("Handling Domain Event: {event} - start at {time} - with message {message}", nameof(EventStartDomainEvent), startTime, $"{notification.@event.Id}|{notification.@event.GameId.Value}");
        }
    }
}
