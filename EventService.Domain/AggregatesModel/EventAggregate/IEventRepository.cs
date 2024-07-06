using EventService.Domain.Interfaces;

namespace EventService.Domain.AggregatesModel.EventAggregate {
    public interface IEventRepository : IRepository<Event> {
        Event AddEvent(Event @event);
        void UpdateEvent(Event @event);
        void DeleteEvent(Event @event);
        Task<Event> GetByIdAsync(long id);
        Task<IEnumerable<Event>> Get();
    }
}
