using EventService.API.Application.ScheduleJob;
using EventService.Domain.Events;
using EventService.Infrastructure;
using MediatR;
using Quartz;

namespace EventService.API.Application.DomainEventHandlers {
    public class EventStartDomainEventHandler : INotificationHandler<EventStartDomainEvent> {
        private EventDbContext _context;
        private ISchedulerFactory _scheduler;
        private ILogger<EventStartDomainEventHandler> _logger;

        public EventStartDomainEventHandler(EventDbContext context, ISchedulerFactory scheduler, ILogger<EventStartDomainEventHandler> logger) {
            _context = context;
            _logger = logger;
            _scheduler = scheduler;
        }

        public async Task Handle(EventStartDomainEvent notification, CancellationToken cancellationToken) {
            var jobKey = new JobKey(nameof(NotifyEventStart));

            _logger.LogInformation("Handling Domain Event: {@event}", nameof(EventStartDomainEvent));
            var scheduler = await _scheduler.GetScheduler();

            if (await scheduler.CheckExists(jobKey)) {
                DateTime startTime = notification.@event.StartDate < DateTime.UtcNow ? DateTime.UtcNow.AddSeconds(10) : notification.@event.StartDate;
                await scheduler.TriggerJob(jobKey);
            }
        }
    }
}
