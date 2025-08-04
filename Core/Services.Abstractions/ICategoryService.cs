using Shared.DTOs.Category;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ICategoryService
    {
        //Get AllCategories
        Task<IEnumerable<CategoryResultDto>> GetCategoriesAsync();

        Task<CategoryResultDto?> GetCategoryByIdAsync(int id);
        Task<int> AddCategoryAsync(CategoryCreateDto dto); // يرجع الـ ID الجديد

        Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
