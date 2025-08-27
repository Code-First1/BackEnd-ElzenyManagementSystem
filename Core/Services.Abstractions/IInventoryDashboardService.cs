using Domain.Models;
using Shared.DTOs.InventoryDashboard;
using Shared.DTOs.InventoryProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IInventoryDashboardService
    {
        Task<InventoryDashboardCountDto> GetProductsCountAsync();

        Task<IEnumerable<InventoryProductResultDto>> GetGoodProductsAsync();
        Task<IEnumerable<InventoryProductResultDto>> GetCriticalProductsAsync();
        Task<IEnumerable<InventoryProductResultDto>> GetEmptyProductsAsync();
    }
}
