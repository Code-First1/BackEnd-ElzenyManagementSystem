using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
    public class InvoiceResultDto
    {
        public int Id { get; set; }
        public IEnumerable<InvoiceProductResultDto> Products { get; set; }
        // other properties...
    }
}
