using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TransationFromInventoryToShopInProduct
{
    public class TransactionPerProductCreateDto
    {
        public int productId {  get; set; }
        public int Quantity { get; set; }
    }
}
