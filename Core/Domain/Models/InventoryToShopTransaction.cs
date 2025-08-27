using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class InventoryToShopTransaction : BaseEntity<int>
    {
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public IEnumerable<InventoryToShopTransactionItem> Items { get; set; }
    }
}
