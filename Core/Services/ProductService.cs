using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared.DTOs.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<ProductResultDto>> GetProductsAsync()
        {
            var spec = new ProductWithCategoriesSpecification();


            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
            return mapper.Map<IEnumerable<ProductResultDto>>(products);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithCategoriesSpecification(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            return product is null ? null : mapper.Map<ProductResultDto>(product);
        }

        public async Task<int> AddProductAsync(ProductCreateDto dto)
        {
            var product = mapper.Map<Product>(dto);

            await unitOfWork.GetRepository<Product, int>().AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var repo = unitOfWork.GetRepository<Product, int>();
            var existing = await repo.GetAsync(id);

            if (existing is null) return false;

            mapper.Map(dto, existing); // تحديث الخصائص باستخدام AutoMapper
            repo.Update(existing);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var repo = unitOfWork.GetRepository<Product, int>();
            var product = await repo.GetAsync(id);

            if (product is null) return false;

            repo.Delete(product);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
