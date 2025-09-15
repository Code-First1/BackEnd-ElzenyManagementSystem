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

        public string UserName { get; set; }

        public DateTime DateTime { get; set; }
        public decimal Total { get; set; }
        public int number { get; set; }
        public IEnumerable<InvoiceProductResultDto> InvoiceProduct { get; set; }
        // other properties...
    }
}
