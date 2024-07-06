using EventService.Domain.AggregatesModel.VoucherAggregate;
using EventService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Repository {
    public class VoucherRepository : IVoucherRepository {
        public IUnitOfWork UnitOfWork => _context;
        private readonly EventContext _context;

        public VoucherRepository(EventContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Voucher AddVoucher(Voucher voucher) {
            if (voucher.IsTransient()) {
                return _context.Vouchers.Add(voucher).Entity;
            }

            return voucher;
        }

        public void DeleteVoucher(Voucher voucher) {
            _context.Vouchers.Remove(voucher);
        }

        public async Task<IEnumerable<Voucher>> Get() {
            var vouchers = await _context.Vouchers.ToListAsync();
            return vouchers;
        }

        public async Task<Voucher> GetByIdAsync(long id) {
            var voucher = await _context.Vouchers.FindAsync(id);
            return voucher;
        }

        public void UpdateVoucher(Voucher voucher) {
            _context.Entry(voucher).State = EntityState.Modified;
        }
    }
}