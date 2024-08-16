namespace EventService.API.Application.Queries {
    public interface IEventQueries {
        Task<BrandVM> GetBrand(int id);
        Task<IEnumerable<EventVM>> GetBrandEvent(int id);
        Task<IEnumerable<VoucherVM>> GetBrandVoucher(int id);
        Task<IEnumerable<RedeemVoucherVM>> GetBrandRedeemVoucher(int id);
        Task<IEnumerable<PlayerVM>> GetBrandPlayer(int id);
        Task<IEnumerable<PlayerVM>> GetBrandPlayerByDate(int id, DateTime start, DateTime end);
        Task<IEnumerable<VoucherVM>> GetEventVoucher(int id);
        Task<IEnumerable<RedeemVoucherVM>> GetEventRedeemVoucher(int id);
        Task<IEnumerable<PlayerVM>> GetEventPlayer(int id);
        Task<EventWithVoucherVM> GetEventWithVoucher(int id);
        Task<IEnumerable<GameVM>> GetGame();
    }
}
