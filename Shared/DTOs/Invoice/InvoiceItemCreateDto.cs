using Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace Shared.DTOs.Invoice
{
    public class InvoiceItemCreateDto
    {
        public int ProductId { get; set; }
        //public decimal UnitPrice { get; set; }
        public bool Typing { get; set; } // 1 for meter or piece 0 for roll or box 
        public int Quantity { get; set; }
      
        //public decimal TotalPricePerItem => UnitPrice * Quantity;
    }
}
