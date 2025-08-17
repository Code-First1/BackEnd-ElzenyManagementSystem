using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.SubCategor
{
    public class SubCategoryUpdateDto
    {
        [Required(ErrorMessage = "SubCategory name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category Id is required.")]
        public int CategoryId { get; set; }
    }
}
