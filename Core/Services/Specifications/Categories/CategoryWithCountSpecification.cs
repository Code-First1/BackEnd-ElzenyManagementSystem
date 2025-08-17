using Domain.Models;
using Shared.SpecificationsParam.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Categories
{
    public class CategoryWithCountSpecification : BaseSpecifications<Category, int>
    {
        public CategoryWithCountSpecification(CategorySpecificationsParameters categorySpecsParams)
            : base(
                  P =>
                  (string.IsNullOrEmpty(categorySpecsParams.Search) || P.Name.ToLower().Contains(categorySpecsParams.Search.ToLower()))
                  )
        {
            
        }
    }
}
