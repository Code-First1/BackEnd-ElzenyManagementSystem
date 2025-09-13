using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
    public class InvoiceCreateResultDto
    {
        public int Id { get; set; }

     
        public List<InvoiceDerivedCreateDto> Items { get; set; } = new();

    }
}
