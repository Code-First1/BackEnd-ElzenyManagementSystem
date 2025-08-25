using Shared.DTOs.InventoryProduct;
using Shared.Response;
using Shared.SpecificationsParam.InventoryProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IInventoryProductService
    {
        Task<PaginationResponse<InventoryProductResultDto>> GetAllAsync(InventoryProductSpecificationsParams specParams);
        Task<InventoryProductResultDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, InventoryProductUpdateDto dto);
    }
}
