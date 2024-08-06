namespace EventService.API.Application.Queries
{
    public interface IEventQueries
    {
        Task<BrandVM> GetBrand(int id);
        Task<IEnumerable<EventVM>> GetBrandEvent(int id);
        Task<IEnumerable<VoucherVM>> GetBrandVoucher(int id);
    }
}
