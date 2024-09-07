using InventoryService.API.Infrastructure;
using InventoryService.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace InventoryService.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> _logger;
        private readonly InventoryDbContext _context;
        private readonly HttpClient _httpClient;

        public UserController(HttpClient httpClient, ILogger<UserController> logger, InventoryDbContext context) {
            _logger = logger;
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet("{id}/Pieces")]
        public async Task<ActionResult<IEnumerable<ItemPiece>>> GetUserItemPieces(Guid id) {
            var user = await _context.Users.Include(u => u.ItemPieces).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return BadRequest();
            }

            return Ok(user.ItemPieces);
        }

        [HttpGet("{id}/Items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetUserItems(Guid id) {
            var user = await _context.Users.Include(u => u.Items).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return BadRequest();
            }

            return Ok(user.Items);
        }

        [HttpGet("{id}/Vouchers")]
        public async Task<ActionResult<IEnumerable<Item>>> GetUserVouchers(Guid id) {
            var user = await _context.Users.Include(u => u.Vouchers).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return BadRequest();

            return Ok(user.Vouchers);
        }

        [HttpPost("{id}/Pieces")]
        public async Task<ActionResult<Guid>> AddPieceToUserInventory(Guid id, AddItemPieceRequest request) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {
                user = new User { Id = id };
                _context.Users.Add(user);
            }

            var piece = user.AddItemPiece(request.itemPieceId, request.itemId, request.eventId);
            await _context.SaveChangesAsync();
            return Ok(piece.Id);
        }

        [HttpPost("{id}/Items")]
        public async Task<ActionResult<Guid>> AddItemToUserInventory(Guid id, AddItemRequest request) {
            var user = await _context.Users.Include(u => u.ItemPieces).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {
                user = new User { Id = id };
                _context.Users.Add(user);
            }

            var item = user.AddItem(request.itemPieceId, request.quantity);
            await _context.SaveChangesAsync();
            return Ok(item.Id);
        }

        [HttpPost("{id}/Vouchers")]
        public async Task<ActionResult<RedeemVoucherVM>> AddVoucherToUserInventory(Guid id, Guid itemId) {
            var user = await _context.Users.Include(u => u.Items).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {
                user = new User { Id = id };
                _context.Users.Add(user);
            }

            var item = user.Items.FirstOrDefault(u => u.Id == itemId);
            if (item == null)
                return BadRequest();

            // Get redeem voucher code from random voucher
            var jsonData = JsonSerializer.Serialize(new {
                eventId = item.EventId
            });

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://localhost:7361/api/Vouchers/Redeem", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var redeemVoucherData = JsonSerializer.Deserialize<RedeemVoucherData>(responseContent, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });

            if (redeemVoucherData == null)
                return BadRequest();

            user.RemoveItem(itemId);
            var voucher = user.AddVoucher(item.EventId, redeemVoucherData.voucherId, redeemVoucherData.code, redeemVoucherData.expireDate);
            await _context.SaveChangesAsync();
            return Ok(new RedeemVoucherVM(voucher.Id, voucher.Code));
        }

        public record RedeemVoucherVM(Guid id, string code);
        public record AddItemPieceRequest(Guid itemPieceId, Guid itemId, int eventId);
        public record AddItemRequest(Guid itemPieceId, int quantity);
        public record RedeemVoucherData(string code, int voucherId, int expireDate);
    }
}
