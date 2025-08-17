using Domain.Models;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Products
{
    public class ProductWithCategoriesAndSubCategoriesSpecification : BaseSpecifications<Product, int>
    {
        public ProductWithCategoriesAndSubCategoriesSpecification(int id) : base(P => P.Id == id)
        {
            ApplyInclude();
        }

        public ProductWithCategoriesAndSubCategoriesSpecification(ProductSpecificationsParamters productSpecsParams)
            : base(
                    P =>
                        (string.IsNullOrEmpty(productSpecsParams.Search)
                            || P.Name.ToLower().Contains(productSpecsParams.Search.ToLower())) &&
                        (!productSpecsParams.CategoryId.HasValue
                            || P.CategoryId == productSpecsParams.CategoryId) &&
                        (productSpecsParams.CategoryId.HasValue
                            ? (!productSpecsParams.SubCategoryId.HasValue
                                || P.SubCategoryId == productSpecsParams.SubCategoryId)
                            : true)
                   )
        {
            ApplyInclude();

            ApplySorting(productSpecsParams.sort);

            ApplyPagination(productSpecsParams.PageIndex, productSpecsParams.PageSize);
        }

        private void ApplyInclude()
        {
            AddInclude(P => P.Category);
            AddInclude(P => P.SubCategory);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.PricePerUnit);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.PricePerUnit);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;

                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }
    }
}
