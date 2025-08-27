using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Specifications.InventoryDashboard
{
    public class GoodProductsSpecification : BaseSpecifications<InventoryProduct, int>
    {
        public GoodProductsSpecification()
            : base(x => x.Quantity > x.MinimumQuantity)
        {
            AddInclude(x => x.Product.Category.SubCategories);
        }
    }
}
