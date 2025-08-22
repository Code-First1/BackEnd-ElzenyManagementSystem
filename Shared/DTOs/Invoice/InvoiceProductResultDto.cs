using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.Invoice
{
    public class InvoiceProductResultDto
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; } 
        public decimal pricePerUnit {  get; set; }
        public int Quantity { get; set; }
    }
}
