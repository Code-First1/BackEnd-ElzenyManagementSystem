using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions.category;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.SubCategories;
using Shared.DTOs.SubCategor;
using Shared.SpecificationsParam.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ISubCategoryService
    {
        public async Task<IEnumerable<SubCategoryResultDto>> GetAllSubCategoriesAsync(SubCategorySpecificationsParameters subcategorySpecsParams)
        {
            var spec = new SubCategorySpecifications(subcategorySpecsParams);
            var subCategories = await unitOfWork.GetRepository<SubCategory, int>().GetAllAsync(spec);
            
            var result = mapper.Map<IEnumerable<SubCategoryResultDto>>(subCategories);
            return result;
        }
        public async Task<SubCategoryResultDto> GetSubCategoryByIdAsync(int id)
        {
            var spec = new SubCategorySpecifications(id);
            var subCategory = await unitOfWork.GetRepository<SubCategory,int>().GetAsync(spec);

            if (subCategory == null)
                throw new SubCategoryNotFoundException(id);

            var result = mapper.Map<SubCategoryResultDto>(subCategory);
            return result;
        }
        public async Task<int> AddSubCategoryAsync(SubCategoryCreateDto dto)
        {
            var category = await unitOfWork.GetRepository<Category,int>().GetAsync(dto.CategoryId);
            if (category == null)
                throw new CategoryNotFoundException(dto.CategoryId);

            var subCategory = new SubCategory
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId
            };

            await unitOfWork.GetRepository<SubCategory, int>().AddAsync(subCategory);
            await unitOfWork.SaveChangesAsync();

            return subCategory.Id;
            
        }

        public async Task<bool> UpdateSubCategoryAsync(int id, SubCategoryUpdateDto dto)
        {
            var category = await unitOfWork.GetRepository<Category, int>().GetAsync(dto.CategoryId);
            if (category == null)
                throw new CategoryNotFoundException(dto.CategoryId);

            var repo = unitOfWork.GetRepository<SubCategory,int>();
            var subCategory = await repo.GetAsync(id);

            if (subCategory == null)
                throw new SubCategoryNotFoundException(id);

            subCategory.Name = dto.Name;
            subCategory.CategoryId = dto.CategoryId;

            repo.Update(subCategory);
            await unitOfWork.SaveChangesAsync();

            return true;
            
        }

        public async Task<bool> DeleteSubCategoryAsync(int id)
        {
            var repo = unitOfWork.GetRepository<SubCategory, int>();
            var subCategory = await repo.GetAsync(id);

            if(subCategory == null)
                throw new SubCategoryNotFoundException(id);

            repo.Delete(subCategory);
            await unitOfWork.SaveChangesAsync();

            return true;
        }



    }
}
