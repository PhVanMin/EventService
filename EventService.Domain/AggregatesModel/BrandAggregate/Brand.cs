﻿using EventService.Domain.AggregatesModel.EventAggregate;
using EventService.Domain.Interfaces;

namespace EventService.Domain.AggregatesModel.BrandAggregate;

public class Brand : IAggregateRoot
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string? Address { get; set; }

    public string? Gps { get; set; }

    public short Status { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
