using Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace Shared.DTOs.Invoice
{
    public class InvoiceItemCreateDto
    {
        public int ProductId { get; set; }
        //public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Unit Unit { get; set; }
        //public decimal TotalPricePerItem => UnitPrice * Quantity;
    }
}
