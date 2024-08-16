using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.PlayerAggregate
{
    public class Player : Entity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime LastAccessed { get; set; }

        private List<EventPlayer> _events;
        public IReadOnlyCollection<EventPlayer> Events => _events.AsReadOnly();
        public Player() { 
            _events = new List<EventPlayer>();
        }
        public Player(string name, string email) : this() {
            Name = name;
            Email = email;
            LastAccessed = DateTime.Now;
        }

        public void UpdateLastAccess()
        {
            LastAccessed = DateTime.Now;
        }
    }
}
