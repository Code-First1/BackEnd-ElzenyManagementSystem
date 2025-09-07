using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
    public class InvoiceDerivedCreateDto
    {
        public string productName { get; set; } = string.Empty; 
        public int productId { get; set; }

        public int? transferCount { get; set; }
    }
}
