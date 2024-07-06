namespace EventService.Domain.SeedWork {
    public class Entity {
        int _id;
        public virtual int Id {
            get {
                return _id;
            }
            protected set {
                _id = value;
            }
        }
        public bool IsTransient() {
            return _id == default;
        }
    }
}
