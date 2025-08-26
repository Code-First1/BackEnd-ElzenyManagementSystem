using Shared.DTOs.ShopProduct;
using Shared.Response;
using Shared.SpecificationsParam.ShopProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IShopProductService
    {
        Task<PaginationResponse<ShopProductResultDto>> GetAllAsync(ShopProductSpecificationsParams specParams);
        Task<ShopProductResultDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, ShopProductUpdateDto dto);
        Task<ShopProductCreateDto> CreateAsync(ShopProductCreateDto dto);
    }
}
