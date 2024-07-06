using EventService.Domain.Interfaces;

namespace EventService.Domain.AggregatesModel.VoucherAggregate {
    public interface IVoucherRepository : IRepository<Voucher> {
        Voucher AddVoucher(Voucher voucher);
        void UpdateVoucher(Voucher voucher);
        void DeleteVoucher(Voucher voucher);
        Task<Voucher> GetByIdAsync(long id);
        Task<IEnumerable<Voucher>> Get();
    }
}
