namespace InventoryService.API.Models {
    public class User {
        public Guid Id { get; set; }
        private List<Item> _items = new List<Item>();
        public IReadOnlyCollection<Item> Items => _items.AsReadOnly();

        private List<ItemPiece> _itemPieces = new List<ItemPiece>();
        public IReadOnlyCollection<ItemPiece> ItemPieces => _itemPieces.AsReadOnly();

        private List<Voucher> _vouchers = new List<Voucher>();
        public IReadOnlyCollection<Voucher> Vouchers => _vouchers.AsReadOnly();

        public ItemPiece AddItemPiece(Guid itemPieceId, Guid itemId, int eventId) {
            var itemPiece = new ItemPiece {
                GameItemId = itemId,
                EventId = eventId,
                ItemPieceId = itemPieceId,
            };

            _itemPieces.Add(itemPiece);
            return itemPiece;
        }

        public void ReceiveItem(Item item) {
            _items.Add(item);
        }

        public Item AddItem(Guid itemPieceId, int quantity) {
            var itemPieces = _itemPieces.Where(x => x.ItemPieceId == itemPieceId).Take(quantity);

            if (itemPieces.Count() < quantity)
                throw new Exception();

            var eventId = itemPieces.First().EventId;
            var itemId = itemPieces.First().GameItemId;

            foreach (var piece in itemPieces) {
                _itemPieces.Remove(piece);
            }

            var item = new Item {
                GameItemId = itemId,
                EventId = eventId,
            };

            _items.Add(item);
            return item;
        }

        public Item? RemoveItem(Guid gameItemId) {
            var item = _items.FirstOrDefault(i => i.GameItemId == gameItemId);
            if (item == null) return null;

            _items.Remove(item);
            return item;
        }

        public Voucher AddVoucher(int eventId, int voucherId, string code, int dateToExpire) {
            var voucher = new Voucher {
                EventId = eventId,
                VoucherId = voucherId,
                Code = code,
                ExpireDate = DateTime.UtcNow.AddDays(dateToExpire)
            };
            _vouchers.Add(voucher);
            return voucher;
        }

        public void RedeemVoucher(Guid voucherId, string code) {
            var voucher = _vouchers.FirstOrDefault(v => v.Id == voucherId);
            if (voucher == null) 
                throw new Exception("User does not own this voucher.");

            if (voucher.ExpireDate < DateTime.UtcNow) 
                throw new Exception("Voucher expired.");

            if (voucher.Code != code)
                throw new Exception("Voucher code does not match.");

            _vouchers.Remove(voucher);
        }
    }
}
