namespace InventoryService.API.Models {
    public class Item {
        public Guid Id { get; set; }
        public Guid GameItemId {  get; set; }
        public int EventId { get; set; }
    }
}
