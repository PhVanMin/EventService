using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace EventService.Domain.Model;
public class Voucher : Entity, IAggregateRoot {
    public string Code { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int Value { get; set; }

    public string Description { get; set; } = null!;

    public DateTime ExpireDate { get; set; }

    public int Status { get; set; }

    public int EventId { get; set; }

    [JsonIgnore]
    public Event Event { get; set; } = null!;
}
