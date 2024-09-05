namespace EventService.API.Application.Queries {
    public interface IEventQueries {
        // Brand Queries
        Task<BrandVM> GetBrand(int id);
        Task<IEnumerable<EventVM>> GetBrandEvent(int id);
        Task<IEnumerable<VoucherVM>> GetBrandVoucher(int id);
        Task<IEnumerable<RedeemVoucherVM>> GetBrandRedeemVoucher(int id);
        Task<BrandStatisticsVM> GetBrandStatisticsByDate(int id, bool all, DateTime start, DateTime end);

        // Event Queries
        Task<IEnumerable<VoucherVM>> GetEventVoucher(int id);
        Task<EventWithVoucherVM> GetEventWithVoucher(int id);
        Task<string> GetEventVoucherCode(int eventId, int voucherId);

        // Voucher Queries
        Task<VoucherVM> GetVoucher(int id);
    }
}
