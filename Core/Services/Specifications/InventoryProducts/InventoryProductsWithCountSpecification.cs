using Domain.Models;
using Shared.SpecificationsParam.InventoryProduct;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.InventoryProducts
{
    public class InventoryProductsWithCountSpecification : BaseSpecifications<InventoryProduct, int>
    {
        public InventoryProductsWithCountSpecification(InventoryProductSpecificationsParams specParams)
            : base(x =>
                // Search by product name
                (string.IsNullOrEmpty(specParams.Search) ||
                    x.Product.Name.ToLower().Contains(specParams.Search.ToLower())) &&

                // Filter by CategoryId (with optional SubCategoryId)
                (!specParams.CategoryId.HasValue ||
                    (x.Product.CategoryId == specParams.CategoryId.Value &&
                        (!specParams.SubCategoryId.HasValue ||
                            x.Product.SubCategoryId == specParams.SubCategoryId.Value))) &&

                // State filter
                (!specParams.State.HasValue ||
                    (specParams.State == 1 && x.Quantity > x.MinimumQuantity) ||   // Good
                    (specParams.State == 2 && x.Quantity <= x.MinimumQuantity && x.Quantity > 0) || // Critical
                    (specParams.State == 3 && x.Quantity == 0)) // Empty
            )
        {

        }
    }
}
