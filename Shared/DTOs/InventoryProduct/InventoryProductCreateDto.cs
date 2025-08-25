using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.InventoryProduct
{
    public class InventoryProductCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
    }
}
