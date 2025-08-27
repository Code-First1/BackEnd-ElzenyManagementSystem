using Domain.Models;
using Shared.SpecificationsParam.ShopProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.ShopProducts
{
    public class ShopProductsWithCountSpecification : BaseSpecifications<ShopProduct, int>
    {
        public ShopProductsWithCountSpecification(ShopProductSpecificationsParams specParams)
            : base(x =>
                // Search by product name
                (string.IsNullOrEmpty(specParams.Search) ||
                    x.Product.Name.ToLower().Contains(specParams.Search.ToLower())) &&

                // Filter by CategoryId (with optional SubCategoryId)
                (!specParams.CategoryId.HasValue ||
                    (x.Product.CategoryId == specParams.CategoryId.Value &&
                        (!specParams.SubCategoryId.HasValue ||
                            x.Product.SubCategoryId == specParams.SubCategoryId.Value)))
            )
        {
            
        }
    }
}
