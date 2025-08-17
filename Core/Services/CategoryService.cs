using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions.category;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.Categories;
using Shared.DTOs.Category;
using Shared.DTOs.Product;
using Shared.Response;
using Shared.SpecificationsParam.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        public async Task<PaginationResponse<CategoryResultDto>> GetCategoriesAsync(CategorySpecificationsParameters categorySpecsParams)
        {
            var spec = new CategoryWithSubCategoriesSpecification(categorySpecsParams);
            var categories = await unitOfWork.GetRepository<Category, int>().GetAllAsync(spec);

            var specCount = new CategoryWithCountSpecification(categorySpecsParams);
            var count = await unitOfWork.GetRepository<Category,int>().CountAsync(specCount);

            var result = mapper.Map<IEnumerable<CategoryResultDto>>(categories);

            return new PaginationResponse<CategoryResultDto>(
                categorySpecsParams.PageIndex,
                categorySpecsParams.PageSize,
                count,
                result
                );
        }

        public async Task<CategoryResultDto?> GetCategoryByIdAsync(int id)
        {
            var spec = new CategoryWithSubCategoriesSpecification(id);
            var category = await unitOfWork.GetRepository<Category, int>().GetAsync(spec);

            if (category == null) throw new CategoryNotFoundException(id);

            var result = mapper.Map<CategoryResultDto>(category);

            return result;
        }

        public async Task<int> AddCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category { Name = dto.Name };

            await unitOfWork.GetRepository<Category, int>().AddAsync(category);
            await unitOfWork.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var repo = unitOfWork.GetRepository<Category, int>();
            var category = await repo.GetAsync(id);

            if (category is null)
                throw new CategoryNotFoundException(id);

            category.Name = dto.Name;
            repo.Update(category);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var repo = unitOfWork.GetRepository<Category, int>();
            var category = await repo.GetAsync(id);

            if (category is null)
                throw new CategoryNotFoundException(id);

            repo.Delete(category);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
