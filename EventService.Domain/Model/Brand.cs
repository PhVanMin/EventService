using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.Model;

public class Brand : Entity, IAggregateRoot
{
    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string? Address { get; set; }

    public string? Gps { get; set; }

    public short Status { get; set; }

    public ICollection<Event> Events { get; set; } = new List<Event>();
}
