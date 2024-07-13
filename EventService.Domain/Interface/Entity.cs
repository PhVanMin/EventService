namespace EventService.Domain.SeedWork {
    public class Entity {
        public long Id { get; set; }

        public bool IsTransient() {
            return Id == default;
        }
    }
}
