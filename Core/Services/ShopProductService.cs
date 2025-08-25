using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions.ShopProduct;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.ShopProducts;
using Shared.DTOs.ShopProduct;
using Shared.Response;
using Shared.SpecificationsParam.ShopProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ShopProductService(IUnitOfWork unitOfWork, IMapper mapper) : IShopProductService
    {
        public async Task<PaginationResponse<ShopProductResultDto>> GetAllAsync(ShopProductSpecificationsParams specParams)
        {
            var spec = new ShopProductWithDetailsSpecification(specParams);
            var products = await unitOfWork.GetRepository<ShopProduct, int>().GetAllAsync(spec);

            //var specCount = new ShopProductsWithCountSpecification(specParams);
            var count = await unitOfWork.GetRepository<ShopProduct, int>().CountAsync(spec);

            var result = mapper.Map<IEnumerable<ShopProductResultDto>>(products);

            return new PaginationResponse<ShopProductResultDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalCount: count,
                result
            );
        }

        public async Task<ShopProductResultDto?> GetByIdAsync(int id)
        {
            var spec = new ShopProductWithDetailsSpecification(id);
            var entity = await unitOfWork.GetRepository<ShopProduct, int>().GetAsync(spec);

            if (entity is null) throw new ShopProductNotFoundException(id);

            return mapper.Map<ShopProductResultDto>(entity);
        }
        public async Task<bool> UpdateAsync(int id, ShopProductUpdateDto dto)
        {
            var repo = unitOfWork.GetRepository<ShopProduct, int>();
            var existing = await repo.GetAsync(id);

            if (existing is null) return false;

            mapper.Map(dto, existing);
            repo.Update(existing);

            await unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
