using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.InventoryDashboard
{
    public class CriticalProductsSpecification : BaseSpecifications<InventoryProduct, int>
    {
        public CriticalProductsSpecification()
            : base(x => x.Quantity <= x.MinimumQuantity && x.Quantity > 0)
        {
            AddInclude(x => x.Product.Category.SubCategories);
        }
    }
}
