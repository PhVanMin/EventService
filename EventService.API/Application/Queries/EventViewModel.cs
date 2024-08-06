namespace EventService.API.Application.Queries
{
    public record BrandVM
    {
        public int Id;
        public string Name = null!;
        public string Field = null!;
        public string Address = null!;
        public string Gps = null!;
        public int Status;
    }

    public record EventVM
    {
        public int Id;
        public string Name = null!;
        public string Image = null!;
        public int NoVoucher;
        public DateTime StartDate;
        public DateTime EndDate;
        public int? GameId;
    }

    public record VoucherVM {
        public int Id;
        public string Code = null!;
        public string Image = null!;
        public int Value;
        public string Description = null!;
        public DateTime ExpireDate;
        public int Status;
    }
}
