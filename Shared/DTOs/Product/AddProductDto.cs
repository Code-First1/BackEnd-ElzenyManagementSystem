using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Product
{
    public class AddProductDto
    {
        public int productId {  get; set; }
        public int inventoryProductId { get; set; }
    }
}
