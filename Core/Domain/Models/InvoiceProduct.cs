using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class InvoiceProduct : BaseEntity<int>
    {

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; } 

        public decimal Total => Quantity * UnitPrice;

        //Forign Keys

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Navigation properties
        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }
}
