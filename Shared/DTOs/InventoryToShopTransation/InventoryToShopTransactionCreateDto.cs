using Shared.DTOs.InventoryToShopTransactionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.InventoryToShopTransation
{
    public class InventoryToShopTransactionCreateDto
    {
        public IEnumerable<InventoryToShopTransactionItemDto> Items { get; set; }
    }
}
