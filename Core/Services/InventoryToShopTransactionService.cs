using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.InventoryProducts;
using Services.Specifications.ShopProducts;
using Shared.DTOs.InventoryToShopTransation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class InventoryToShopTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    : IInventoryToShopTransactionService
    {
        public async Task<int> CreateTransactionAsync(InventoryToShopTransactionCreateDto dto)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var transactionEntity = new InventoryToShopTransaction();
                await unitOfWork.GetRepository<InventoryToShopTransaction, int>().AddAsync(transactionEntity);
                await unitOfWork.SaveChangesAsync();

                foreach (var item in dto.Items)
                {
                    var inventoryRepo = unitOfWork.GetRepository<InventoryProduct, int>();
                    var shopRepo = unitOfWork.GetRepository<ShopProduct, int>();

                    var inventoryProductSpec = new InventoryProductDeleteSpecification(item.ProductId);
                    var inventoryProduct = await inventoryRepo.GetAsync(inventoryProductSpec);
                    if (inventoryProduct == null || inventoryProduct.Quantity < item.Quantity)
                        throw new Exception($"Insufficient inventory for product {item.ProductId}");

                    // decrease from inventory
                    inventoryProduct.Quantity -= item.Quantity;
                    inventoryRepo.Update(inventoryProduct);

                    // increase in shop
                    var shopProductSpec = new ShopProductDeleteSpecification(item.ProductId);
                    var shopProduct = await shopRepo.GetAsync(shopProductSpec);
                    if (shopProduct == null)
                    {
                        shopProduct = new ShopProduct
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        };
                        await shopRepo.AddAsync(shopProduct);
                    }
                    else
                    {
                        shopProduct.Quantity += item.Quantity;
                        shopRepo.Update(shopProduct);
                    }

                    // add transaction item
                    var transactionItem = new InventoryToShopTransactionItem
                    {
                        TransactionId = transactionEntity.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };
                    await unitOfWork.GetRepository<InventoryToShopTransactionItem, int>().AddAsync(transactionItem);
                }

                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return transactionEntity.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
