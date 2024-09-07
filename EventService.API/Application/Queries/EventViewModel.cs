namespace EventService.API.Application.Queries {
    public record BrandVM {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public string Field { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Gps { get; set; } = null!;
        public int Status { get; set; }
    }

    public record EventVM {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int NoVoucher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? GameId { get; set; }
    }

    public record EventWithVoucherVM {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int NoVoucher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? GameId { get; set; }
        public List<VoucherVM> Vouchers { get; set; } = [];
    }

    public record VoucherVM {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
        public int Value { get; set; }
        public string? Description { get; set; }
        public int ExpireDate { get; set; }
        public int Status { get; set; }
    }

    public record PlayerStatisticsVM {
        public long Count { get; set; }
        public long OnlineCount { get; set; }
    }

    public record RedeemVoucherStatisticsVM {
        public long Total { get; set; }
        public long RedeemCount { get; set; }
        public long TotalValue { get; set; }
    }

    public record EventWithStatisticsVM {
        public PlayerStatisticsVM? PlayerData { get; set; }
        public RedeemVoucherStatisticsVM? RedeemVoucherData { get; set; }
    }

    public record BrandStatisticsVM {
        public PlayerStatisticsVM? PlayerData { get; set; }
        public RedeemVoucherStatisticsVM? RedeemVoucherData { get; set; }
        public List<EventWithStatisticsVM>? TopEvents { get; set; }
        public int EventCount { get; set; }
    }

    public record RedeemVoucherVM {
        public int Id { get; set; }
        public long Value { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
