using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace EventService.Domain.Model;
public class Voucher : Entity, IAggregateRoot {
    public string Code { get; set; } = null!;

    public string Image { get; set; } = null!;

    public long Value { get; set; }

    public string Description { get; set; } = null!;

    public DateTime ExpireDate { get; set; }

    public short Status { get; set; }

    public long EventId { get; set; }

    [JsonIgnore]
    public Event Event { get; set; } = null!;
}
