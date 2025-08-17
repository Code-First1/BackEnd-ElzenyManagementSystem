using Shared.DTOs.Category;
using Shared.DTOs.Product;
using Shared.Response;
using Shared.SpecificationsParam.Category;
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
        Task<PaginationResponse<CategoryResultDto>> GetCategoriesAsync(CategorySpecificationsParameters categorySpecsParams);

        Task<CategoryResultDto?> GetCategoryByIdAsync(int id);
        Task<int> AddCategoryAsync(CategoryCreateDto dto); // يرجع الـ ID الجديد

        Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
