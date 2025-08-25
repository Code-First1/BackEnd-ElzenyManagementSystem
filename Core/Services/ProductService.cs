using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions.product;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Services.Specifications.InventoryProducts;
using Services.Specifications.Products;
using Services.Specifications.ShopProducts;
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

            return new PaginationResponse<ProductResultDto>(
                productSpecsParams.PageIndex,
                productSpecsParams.PageSize,
                totalCount:count,
                result);
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

            // إضافة المنتج
            await unitOfWork.GetRepository<Product, int>().AddAsync(product);
            await unitOfWork.SaveChangesAsync(); // نحتاج حفظ أول مرة عشان ناخد product.Id

            // إنشاء InventoryProduct للمنتج الجديد
            var inventoryProduct = new InventoryProduct
            {
                ProductId = product.Id,
                Quantity = 0,
                MinimumQuantity = 0
            };

            await unitOfWork.GetRepository<InventoryProduct, int>().AddAsync(inventoryProduct);

            // إنشاء ShopProduct للمنتج الجديد (بـ default values)
            var shopProduct = new ShopProduct
            {
                ProductId = product.Id,
                Quantity = 0,
                OpenedRollRemaining = 0,
                SmallBoxesPerBigBox = 0,
                FullBigBoxesCount = 0,
                OpenedBigBoxRemaining = 0,
                ShopId = 1 // ⚠️ لو عندك multiple shops، لازم تمرر shopId من مكان تاني
            };
            await unitOfWork.GetRepository<ShopProduct, int>().AddAsync(shopProduct);

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
            var productRepo = unitOfWork.GetRepository<Product, int>();
            var inventoryProductRepo = unitOfWork.GetRepository<InventoryProduct, int>();
            var shopProductRepo = unitOfWork.GetRepository<ShopProduct, int>();

            var product = await productRepo.GetAsync(id);

            if (product is null) return false;

            // Get related InventoryProduct (if exists)
            var inventoryProductSpec = new InventoryProductDeleteSpecification(id);

            var inventoryProduct = await inventoryProductRepo.GetAsync(inventoryProductSpec);
            if (inventoryProduct != null)
            {
                inventoryProductRepo.Delete(inventoryProduct);
            }

            var shopProductSpec = new ShopProductDeleteSpecification(id);
            // Get related ShopProduct (if exists)
            var shopProduct = await shopProductRepo.GetAsync(shopProductSpec);
            if (shopProduct != null)
            {
                shopProductRepo.Delete(shopProduct);
            }

            // Delete Product itself
            productRepo.Delete(product);

            await unitOfWork.SaveChangesAsync();

            return true;
        }


    }
}
