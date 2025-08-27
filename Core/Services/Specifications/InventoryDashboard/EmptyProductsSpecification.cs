using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.InventoryDashboard
{
    public class EmptyProductsSpecification : BaseSpecifications<InventoryProduct, int>
    {
        public EmptyProductsSpecification()
            : base(x => x.Quantity == 0)
        {
            AddInclude(x => x.Product.Category.SubCategories);
        }
    }
}
