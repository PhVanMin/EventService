using InventoryService.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase {
        private readonly ILogger<InventoryController> _logger;
        private readonly InventoryDbContext _context;

        public InventoryController(ILogger<InventoryController> logger, InventoryDbContext context) {
            _logger = logger;
            _context = context;
        }

        [HttpDelete("Redeem")]
        public async Task<ActionResult<bool>> RedeemVoucher(RedeemVoucherRequest request) {
            var user = await _context.Users.Include(u => u.Vouchers).FirstOrDefaultAsync(u => u.Id == request.userId);

            if (user == null) {
                return NotFound();
            }

            try {
                user.RedeemVoucher(request.voucherId, request.code);
                await _context.SaveChangesAsync();
                return true;
            } catch {
                return false;
            }
        }

        [HttpPost("Gift")]
        public async Task<ActionResult<bool>> GiftVoucher(GiftVoucherRequest request) {
            var user = await _context.Users.Include(u => u.Vouchers).FirstOrDefaultAsync(u => u.Id == request.userId);
            var userToGift = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userToGiftId);

            if (user == null)
                return BadRequest();

            if (userToGift == null) {
                userToGift = new Models.User { Id = request.userToGiftId };
                _context.Users.Add(userToGift);
            }

            var item = user.RemoveItem(request.itemId);
            if (item == null)
                return BadRequest();

            userToGift.ReceiveItem(item);

            try {
                await _context.SaveChangesAsync();
                return true;
            } catch {
                return false;
            }
        }

        public record RedeemVoucherRequest(Guid voucherId, Guid userId, string code);
        public record GiftVoucherRequest(Guid itemId, Guid userId, Guid userToGiftId);
    }
}
