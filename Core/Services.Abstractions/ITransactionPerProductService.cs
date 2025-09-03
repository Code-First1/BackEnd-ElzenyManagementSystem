using Shared.DTOs.InventoryToShopTransation;
using Shared.DTOs.TransationFromInventoryToShopInProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Abstractions
{
    public interface ITransactionPerProductService
    {
        Task<TransactionPerProductResultDto> CreateTransactionAsync(TransactionPerProductCreateDto dto);

    }
}
