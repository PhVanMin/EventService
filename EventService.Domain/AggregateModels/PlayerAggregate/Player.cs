using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.PlayerAggregate
{
    public class Player : Entity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        private List<EventPlayer> _events = [];
        public IReadOnlyCollection<EventPlayer> Events => _events.AsReadOnly();

        public Player(string name, string email) {
            Name = name;
            Email = email;
        }
    }
}
