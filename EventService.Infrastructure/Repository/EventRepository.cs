using EventService.Domain.AggregatesModel.EventAggregate;
using EventService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Repository {
    public class EventRepository : IEventRepository {
        private readonly EventContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public EventRepository(EventContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Event AddEvent(Event @event) {
            if (@event.IsTransient())
                return _context.Events.Add(@event).Entity;

            return @event;
        }

        public void DeleteEvent(Event @event) {
            _context.Events.Remove(@event);
        }

        public async Task<IEnumerable<Event>> Get() {
            var events = await _context.Events.ToListAsync();
            return events;
        }

        public async Task<Event> GetByIdAsync(long id) {
            var @event = await _context.Events.FindAsync(id);

            if (@event != null) {
                await _context.Entry(@event).Collection(e => e.Vouchers).LoadAsync();
            }

            return @event;
        }

        public void UpdateEvent(Event @event) {
            _context.Entry(@event).State = EntityState.Modified;
        }
    }
}
