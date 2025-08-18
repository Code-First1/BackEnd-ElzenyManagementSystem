using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions.product;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Services.Specifications.Products;
using Shared.DTOs.Product;
using Shared.Response;
using Shared.SpecificationsParam.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<PaginationResponse<ProductResultDto>> GetProductsAsync(ProductSpecificationsParamters productSpecsParams)
        {
            var spec = new ProductWithCategoriesAndSubCategoriesSpecification(productSpecsParams);


            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

            var specCount = new ProductsWithCountSpecifications(productSpecsParams);

            var count = await unitOfWork.GetRepository<Product,int>().CountAsync(specCount);

            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PaginationResponse<ProductResultDto>(productSpecsParams.PageIndex,productSpecsParams.PageSize,totalCount:count,result);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithCategoriesAndSubCategoriesSpecification(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundExceptions(id);
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

            mapper.Map(dto, existing);
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
