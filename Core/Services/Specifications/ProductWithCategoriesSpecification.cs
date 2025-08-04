using Domain.Models;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithCategoriesSpecification : BaseSpecifications<Product,int>
    {
        public ProductWithCategoriesSpecification(int id) : base(P => P.Id == id)
        {
            ApplyInclude();
        }

        public ProductWithCategoriesSpecification(int? categoryId, string? sort, int pageIndex = 1, int pageSize = 5) 
            : base(
                  p => (!categoryId.HasValue || p.CategoryId == categoryId)
                  )
        {
            ApplyInclude();

            ApplySorting(sort);

            ApplyPagination(pageIndex, pageSize);
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
