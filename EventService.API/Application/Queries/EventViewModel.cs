namespace EventService.API.Application.Queries
{
    public record BrandVM
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public string Field { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Gps { get; set; } = null!;
        public int Status { get; set; }
    }

    public record EventVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int NoVoucher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? GameId { get; set; }
    }

    public record EventWithVoucherVM {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int NoVoucher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? GameId { get; set; }
        public List<VoucherVM> Vouchers { get; set; } = new List<VoucherVM>();
    }

    public record VoucherVM {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public int Value { get; set; }
        public string Description { get; set; } = null!;
        public int ExpireDate { get; set; }
        public int Status { get; set; }
    }

    public record GameVM {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public record PlayerVM {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public record RedeemVoucherVM {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public int Value { get; set; }
        public string Description { get; set; } = null!;
        public string RedeemCode { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
        public DateTime RedeemTime { get; set; }
    }
}
