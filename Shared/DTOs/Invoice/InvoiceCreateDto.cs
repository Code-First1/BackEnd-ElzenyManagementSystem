using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
   
        public class InvoiceCreateDto
        {
            [Required]
            public int ShopId { get; set; }

            [Required]
            public string UserId { get; set; }

          
            public decimal TotalPrice { get; set; }

           
            [Required]
            public List<InvoiceItemCreateDto> Items { get; set; } = new();
        }
}
