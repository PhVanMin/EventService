using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.VoucherAggregate;
public class Voucher : Entity, IAggregateRoot {
    public string Image { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int Value { get; set; }
    public string? Description { get; set; }
    public int ExpireDate { get; set; }
    public int Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public void Update(string? image, string? code, int? value, string? description, int? expireDate, int? status) {
        if (image != null) Image = image;
        if (code != null) Code = code;
        if (value != null) Value = value.Value;
        if (description != null) Description = description;
        if (expireDate != null) ExpireDate = expireDate.Value;
        if (status != null) Status = status.Value;
    }
}
