using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
    public class InvoiceProductResultDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; } 
        public int Quantity { get; set; }
    }
}
