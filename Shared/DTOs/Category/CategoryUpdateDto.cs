using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Category
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
