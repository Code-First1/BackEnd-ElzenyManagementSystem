using Domain.Contracts;
using Domain.Exceptions.product;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.InventoryProducts;
using Services.Specifications.ShopProducts;
using Shared.DTOs.TransationFromInventoryToShopInProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TransactionPerProductService(IUnitOfWork unitOfWork) : ITransactionPerProductService
    {
        public async Task<TransactionPerProductResultDto> CreateTransactionAsync(TransactionPerProductCreateDto dto)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                var inventoryRepo = unitOfWork.GetRepository<InventoryProduct, int>();
                var shopRepo = unitOfWork.GetRepository<ShopProduct, int>();

                var produt = await productRepo.GetAsync(dto.productId);
                if (produt == null)
                    throw new ProductNotFoundExceptions(dto.productId);
                

                var inventoryProductSpec = new InventoryProductDeleteSpecification(dto.productId);
                var inventoryProduct = await inventoryRepo.GetAsync(inventoryProductSpec);

                if (inventoryProduct == null || inventoryProduct.Quantity < dto.Quantity)
                    throw new Exception($"Insufficient inventory for product {dto.productId}");

                inventoryProduct.Quantity -= dto.Quantity;
                inventoryRepo.Update(inventoryProduct);

                var QuantityMapping = dto.Quantity * produt.QuantityForOrigin;

                var shopProductSpec = new ShopProductDeleteSpecification(dto.productId);
                var shopProduct = await shopRepo.GetAsync(shopProductSpec);
                if (shopProduct == null)
                {
                    shopProduct = new ShopProduct
                    {
                        ProductId = dto.productId,
                        Quantity = QuantityMapping
                    };
                    await shopRepo.AddAsync(shopProduct);
                }
                else
                {
                    shopProduct.Quantity += QuantityMapping;
                    shopRepo.Update(shopProduct);
                }

                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TransactionPerProductResultDto()
                {
                    productName = produt.Name,
                    QuantityChanged = dto.Quantity,
                    InventoryQuntityAfter = inventoryProduct.Quantity,
                    ShopQuantityAfter = shopProduct.Quantity
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
