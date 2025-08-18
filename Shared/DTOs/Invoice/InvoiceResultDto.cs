using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
    public  class InvoiceResultDto
    {

        public int Id { get; set; }


        public DateTime CreateAt { get; set; } = DateTime.UtcNow;


        public decimal TotalPrice { get; set; }

        //Forign Keys
        public int ShopId { get; set; }
        public string UserId { get; set; }


        // Navigation Property 
        public IEnumerable<InvoiceItemCreateDto> InvoiceProducts { get; set; }
 

    }
}
