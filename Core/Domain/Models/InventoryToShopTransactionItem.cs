namespace Domain.Models
{
    public class InventoryToShopTransactionItem : BaseEntity<int>
    {
        public int TransactionId { get; set; }
        public InventoryToShopTransaction Transaction { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}