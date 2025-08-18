using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Invoice
{
    public class InvoiceItemCreateDto
    {
        public int? ProductId { get; set; }


        public string ProductName { get; set; }

       
        public decimal UnitPrice { get; set; }

        
        public int Quantity { get; set; }

    
        public decimal TotalPricePerItem => UnitPrice * Quantity;
    }
}
