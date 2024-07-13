using MediatR;
using System.Text.Json.Serialization;

namespace EventService.Domain.SeedWork {
    public class Entity {
        public int Id { get; protected set; }

        private List<INotification> _domainEvents = null!;
        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public bool IsTransient() {
            return Id == default;
        }

        public void AddDomainEvent(INotification eventItem) {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem) {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents() {
            _domainEvents?.Clear();
        }
    }
}
