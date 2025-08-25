using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.InventoryProducts
{
    public class InventoryProductDeleteSpecification : BaseSpecifications<InventoryProduct, int>
    {
        public InventoryProductDeleteSpecification(int productId)
            : base(x => x.ProductId == productId)
        {

        }
    }
}
