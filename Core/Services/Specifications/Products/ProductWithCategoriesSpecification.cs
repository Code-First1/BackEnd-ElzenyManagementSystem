using Domain.Models;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Products
{
    public class ProductWithCategoriesSpecification : BaseSpecifications<Product,int>
    {
        public ProductWithCategoriesSpecification(int id) : base(P => P.Id == id)
        {
            ApplyInclude();
        }

        public ProductWithCategoriesSpecification(ProductSpecificationsParamters productSpecsParams) 
            : base(
                  P =>
                    (string.IsNullOrEmpty(productSpecsParams.Search) || P.Name.ToLower().Contains(productSpecsParams.Search.ToLower()))&&
                    (!productSpecsParams.CategoryId.HasValue || P.CategoryId == productSpecsParams.CategoryId)
                  )
        {
            ApplyInclude();

            ApplySorting(productSpecsParams.sort);

            ApplyPagination(productSpecsParams.PageIndex, productSpecsParams.PageSize);
        }

        private void ApplyInclude()
        {
            AddInclude(P => P.Category);
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
