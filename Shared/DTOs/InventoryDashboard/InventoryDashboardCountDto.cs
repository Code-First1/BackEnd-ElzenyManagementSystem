using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.InventoryDashboard
{
    public class InventoryDashboardCountDto
    {
        public int TotalProductsCount { get; set; }
        public int GoodProductsCount { get; set; }
        public int CriticalProductsCount { get; set; }
        public int EmptyProductsCount { get; set; }
    }
}
