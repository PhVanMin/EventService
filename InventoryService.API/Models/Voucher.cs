namespace InventoryService.API.Models {
    public class Voucher {
        public Guid Id { get; set; }
        public int EventId { get; set; }
        public int VoucherId { get; set; }
        public string Code { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
    }
}
