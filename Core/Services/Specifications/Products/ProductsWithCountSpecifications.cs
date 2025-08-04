using Domain.Models;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Products
{
    public class ProductsWithCountSpecifications : BaseSpecifications<Product,int>
    {
        public ProductsWithCountSpecifications(ProductSpecificationsParamters productSpecsParams)
            : base(
                  p => !productSpecsParams.CategoryId.HasValue || p.CategoryId == productSpecsParams.CategoryId
                  )
        {
            
        }
    }
}
