using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Shared.DTOs.Category;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        public async Task<IEnumerable<CategoryResultDto>> GetCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<Category, int>().GetAllAsync();

            var result = mapper.Map<IEnumerable<CategoryResultDto>>(categories);

            return result;
        }

        public async Task<CategoryResultDto?> GetCategoryByIdAsync(int id)
        {
            var category = await unitOfWork.GetRepository<Category, int>().GetAsync(id);

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

            if (category is null) return false;

            category.Name = dto.Name;
            repo.Update(category);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var repo = unitOfWork.GetRepository<Category, int>();
            var category = await repo.GetAsync(id);

            if (category is null) return false;

            repo.Delete(category);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
