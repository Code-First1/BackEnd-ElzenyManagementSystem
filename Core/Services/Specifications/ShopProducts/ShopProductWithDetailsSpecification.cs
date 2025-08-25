using Domain.Models;
using Shared.SpecificationsParam.ShopProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.ShopProducts
{
    public class ShopProductWithDetailsSpecification : BaseSpecifications<ShopProduct, int>
    {
        public ShopProductWithDetailsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Product.Category.SubCategories);
            AddInclude(x => x.Shop);
        }

        public ShopProductWithDetailsSpecification(ShopProductSpecificationsParams specParams)
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
            AddInclude(x => x.Product.Category.SubCategories);
            AddInclude(x => x.Shop);

            ApplySorting(specParams.Sort);
            ApplyPagination(specParams.PageIndex, specParams.PageSize);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "quantitydesc":
                        AddOrderByDescending(x => x.Quantity);
                        break;
                    case "quantityasc":
                        AddOrderBy(x => x.Quantity);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(x => x.Product.PricePerUnit);
                        break;
                    case "priceasc":
                        AddOrderBy(x => x.Product.PricePerUnit);
                        break;
                    case "namedesc":
                        AddOrderByDescending(x => x.Product.Name);
                        break;
                    case "nameasc":
                        AddOrderBy(x => x.Product.Name);
                        break;
                    default:
                        AddOrderBy(x => x.Product.Name); // default sort by Name Asc
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.Product.Name);
            }
        }
    }

}
