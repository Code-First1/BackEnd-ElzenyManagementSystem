using Shared.DTOs.Category;
using Shared.DTOs.SubCategor;
using Shared.SpecificationsParam.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryResultDto>> GetAllSubCategoriesAsync(SubCategorySpecificationsParameters subcategorySpecsParams);
        Task<SubCategoryResultDto> GetSubCategoryByIdAsync(int id);
        Task<int> AddSubCategoryAsync(SubCategoryCreateDto dto);
        Task<bool> UpdateSubCategoryAsync(int id, SubCategoryUpdateDto dto);
        Task<bool> DeleteSubCategoryAsync(int id);
    }
}
