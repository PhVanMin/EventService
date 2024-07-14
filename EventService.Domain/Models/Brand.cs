using EventService.Domain.Exceptions;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.Model;

public class Brand : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string? Address { get; set; }

    public string? Gps { get; set; }

    public int Status { get; set; }

    private List<Event> _events;
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    public Brand() {
        _events = new List<Event>();
    }

    public void AddEvent(string name, string image, int noVoucher, DateTime start, DateTime end, int gameId) {
        var @event = _events.FirstOrDefault(e => e.Name == name);

        if (@event != null) {
            throw new EventDomainException("$Event with name \"{ name }\" already exists.");
        }

        @event = new Event() {
            Name = name,
            Image = image,
            NoVoucher = noVoucher,
            StartDate = start,
            EndDate = end,
            GameId = gameId
        };

        _events.Add(@event);
    }
}
