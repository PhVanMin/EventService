namespace InventoryService.API.Models {
    public class ItemPiece {
        public Guid Id { get; set; }
        public Guid ItemPieceId { get; set; }
        public Guid GameItemId { get; set; }
        public int EventId { get; set; }
    }
}
