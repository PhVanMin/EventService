using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace EventService.Domain.Model;

public class Event : Entity, IAggregateRoot {
    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public long NoVoucher { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public long BrandId { get; set; }

    public long? GameId { get; set; }

    [JsonIgnore]
    public Brand Brand { get; set; } = null!;

    public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
