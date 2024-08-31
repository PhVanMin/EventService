using EventService.Domain.AggregateModels.BrandAggregate;
using EventService.Domain.Interfaces;
using EventService.Domain.SeedWork;

namespace EventService.Domain.AggregateModels.VoucherAggregate;
public class Voucher : Entity, IAggregateRoot {
    public string Image { get; set; } = null!;
    public int Value { get; set; }
    public string Description { get; set; } = null!;
    public int ExpireDate { get; set; }
    public int Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public void Update(string image, int value, string description, int expireDate, int status) {
        Image = image;
        Value = value;
        Description = description;
        ExpireDate = expireDate;
        Status = status;
    }

    public string GenerateCode() {
        Random random = new Random();
        DateTime timeValue = DateTime.MinValue;
        int rand = random.Next(3600) + 1;
        timeValue = timeValue.AddMinutes(rand);
        byte[] b = BitConverter.GetBytes(timeValue.Ticks);

        string voucherCode = Convert.ToBase64String(b);
        voucherCode = voucherCode.TrimEnd('=');
        voucherCode = voucherCode.Replace('+', '-').Replace('/', '_');
        voucherCode = voucherCode.Substring(0, 9);

        return string.Format("{0}-{1}-{2}",
                          voucherCode.Substring(0, 3),
                          voucherCode.Substring(3, 3),
                          voucherCode.Substring(6, 3));
    }
}
