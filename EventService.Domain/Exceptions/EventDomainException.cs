namespace EventService.Domain.Exceptions {
    public class EventDomainException : Exception {
        public EventDomainException() { }
        public EventDomainException(string message) : base(message) { }
        public EventDomainException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
