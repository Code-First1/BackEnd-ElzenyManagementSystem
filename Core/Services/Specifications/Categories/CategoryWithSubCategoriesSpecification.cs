using Domain.Models;
using Shared.SpecificationsParam.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Categories
{
    public class CategoryWithSubCategoriesSpecification : BaseSpecifications<Category,int>
    {
        public CategoryWithSubCategoriesSpecification(int id) : base(P => P.Id == id)
        {
            ApplyInclude();
        }
        public CategoryWithSubCategoriesSpecification(CategorySpecificationsParameters categorySpecsParams)
            : base(
                  P =>
                  (string.IsNullOrEmpty(categorySpecsParams.Search) || P.Name.ToLower().Contains(categorySpecsParams.Search.ToLower()))
                  )
        {
            ApplyInclude();

            ApplySorting(categorySpecsParams.sort);

            ApplyPagination(categorySpecsParams.PageIndex, categorySpecsParams.PageSize);
            
        }

        private void ApplyInclude()
        {
            AddInclude(P => P.SubCategories);
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
