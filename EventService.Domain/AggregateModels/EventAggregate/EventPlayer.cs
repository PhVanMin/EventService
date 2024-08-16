using EventService.Domain.AggregateModels.PlayerAggregate;

namespace EventService.Domain.AggregateModels.EventAggregate {
    public class EventPlayer {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
