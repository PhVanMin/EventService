using Quartz;

namespace EventService.API.Application.ScheduleJob {
    public class NotifyEventStart : IJob {
        public Task Execute(IJobExecutionContext context) {
            Console.WriteLine("Haha");
            return Task.CompletedTask;
        }
    }
}
