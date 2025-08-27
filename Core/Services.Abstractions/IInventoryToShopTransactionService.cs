using Shared.DTOs.InventoryToShopTransation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IInventoryToShopTransactionService
    {
        Task<int> CreateTransactionAsync(InventoryToShopTransactionCreateDto dto);
    }
}
