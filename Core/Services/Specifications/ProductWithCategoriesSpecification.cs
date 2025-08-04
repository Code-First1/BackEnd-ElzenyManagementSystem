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

        public ProductWithCategoriesSpecification() : base(null)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            AddInclude(P => P.Category);
        }

    }
}
