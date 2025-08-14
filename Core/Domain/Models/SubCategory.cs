using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SubCategory : BaseEntity<int>
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        //Forign Keys
        public int CategoryId { get; set; }

        //Navigation Properties

        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
