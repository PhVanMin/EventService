using EventService.Domain.AggregateModels.EventAggregate;
using EventService.Domain.Exceptions;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace EventService.Domain.AggregateModels.BrandAggregate;

public class Brand : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    [Required]
    public Location Location { get; set; } = null!;

    public int Status { get; set; }

    private List<Event> _events;
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    public Brand() {
        _events = new List<Event>();
    }

    public void Update(string name, string field, short status) {
        Name = name;
        Field = field;
        Status = status;
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
