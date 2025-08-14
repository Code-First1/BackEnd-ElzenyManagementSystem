using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ShopProduct : BaseEntity<int>
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int OpenedRollRemaining { get; set; }
    }
}