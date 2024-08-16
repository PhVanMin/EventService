using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.GameAggregate {
    public class Game : Entity {
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}
