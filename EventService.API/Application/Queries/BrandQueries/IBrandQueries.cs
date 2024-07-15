namespace EventService.API.Application.Queries.BrandQueries {
    public interface IBrandQueries {
        Task<BrandViewModel> GetBrand(int id);
        Task<IEnumerable<BrandViewModel>> GetAllBrand();
    }
}
