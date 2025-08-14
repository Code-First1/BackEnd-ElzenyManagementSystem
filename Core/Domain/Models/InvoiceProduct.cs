using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class InvoiceProduct : BaseEntity<int>
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal PriceAtSale { get; set; } 

        // Navigation properties
        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }
}
