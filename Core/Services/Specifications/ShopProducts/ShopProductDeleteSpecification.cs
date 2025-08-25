using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.ShopProducts
{
    public class ShopProductDeleteSpecification : BaseSpecifications<ShopProduct, int>
    {
        public ShopProductDeleteSpecification(int productId)
            : base(x => x.ProductId == productId)
        {

        }
    }
}
