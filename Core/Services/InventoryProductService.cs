using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions.InventoryProduct;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.InventoryProducts;
using Shared.DTOs.InventoryProduct;
using Shared.Response;
using Shared.SpecificationsParam.InventoryProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class InventoryProductService(IUnitOfWork unitOfWork, IMapper mapper) : IInventoryProductService
    {
        public async Task<PaginationResponse<InventoryProductResultDto>> GetAllAsync(InventoryProductSpecificationsParams specParams)
        {
            var spec = new InventoryProductWithProductSpecification(specParams);
            var products = await unitOfWork.GetRepository<InventoryProduct, int>().GetAllAsync(spec);

            //var specCount = new InventoryProductsWithCountSpecification(specParams);
            var count = await unitOfWork.GetRepository<InventoryProduct, int>().CountAsync(spec);

            var result = mapper.Map<IEnumerable<InventoryProductResultDto>>(products);

            return new PaginationResponse<InventoryProductResultDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalCount: count,
                result
            );
        }

        public async Task<InventoryProductResultDto?> GetByIdAsync(int id)
        {
            var spec = new InventoryProductWithProductSpecification(id);
            var entity = await unitOfWork.GetRepository<InventoryProduct, int>().GetAsync(spec);

            if (entity is null) throw new InventoryProductNotFoundException(id);

            return mapper.Map<InventoryProductResultDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, InventoryProductUpdateDto dto)
        {
            var repo = unitOfWork.GetRepository<InventoryProduct, int>();
            var existing = await repo.GetAsync(id);

            if (existing is null) return false;

            mapper.Map(dto, existing);
            repo.Update(existing);

            await unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
