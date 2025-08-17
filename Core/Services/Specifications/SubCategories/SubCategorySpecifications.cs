using Domain.Models;
using Shared.SpecificationsParam.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.SubCategories
{
    public class SubCategorySpecifications : BaseSpecifications<SubCategory, int>
    {
        public SubCategorySpecifications(int id) : base(P => P.Id == id)
        {

        }
        public SubCategorySpecifications(SubCategorySpecificationsParameters subcategorySpecsParams)
            : base(
                  P =>
                  (P.CategoryId == subcategorySpecsParams.CategoryId)
                  )
        {
            ApplySorting(subcategorySpecsParams.sort);
            
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
