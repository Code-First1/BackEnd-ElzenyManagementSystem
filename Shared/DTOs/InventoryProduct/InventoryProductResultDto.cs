using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.InventoryProduct
{
    public class InventoryProductResultDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }

        public ProductResultDto Product {  get; set; }
    }
}
