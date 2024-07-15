namespace EventService.API.Application.Queries.BrandQueries {
    public record BrandViewModel {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Field { get; set; } = null!;
        public string? Address { get; set; }
        public string? Gps { get; set; }
        public int Status { get; set; }
        public List<BrandEventViewModel> Events { get; set; } = null!;
    }

    public record BrandEventViewModel {
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int NoVoucher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? GameId { get; set; }
    }
}
