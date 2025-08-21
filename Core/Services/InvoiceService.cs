using AutoMapper;
using Domain.Contracts;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Specifications.Invoices;
using Shared.DTOs.Invoice;
using Shared.Response;
using Shared.SpecificationsParam.Invoice;

namespace Services
{
    public class InvoiceService(IUnitOfWork unitOfWork, IMapper mapper) : IInvoiceService
    {

        public async Task<int> AddInvoiceAsync([FromBody]InvoiceCreateDto dto)
        {
            var invoice = new Invoice
            {
                ShopId = dto.ShopId,
                UserId = dto.UserId,
                CreateAt = DateTime.UtcNow,
                InvoiceProducts = new List<InvoiceProduct>()
            };

            var productRepo = unitOfWork.GetRepository<Product, int>();
            var inventoryRepo = unitOfWork.GetRepository<InventoryProduct, int>();
            var shopRepo= unitOfWork.GetRepository<Shop, int>();
            var shopProductRepo = unitOfWork.GetRepository<ShopProduct, int>();

            var allInventory = await inventoryRepo.GetAllAsync();
            var allShop= await shopRepo.GetAllAsync();

            foreach (var itemDto in dto.Items)
            {
                
                var product = await productRepo.GetAsync(itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product with Id {itemDto.ProductId} not found");
                if(itemDto.Unit== Unit.Roll)
                {
                    var inventoryProduct = allInventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (inventoryProduct == null)
                        throw new Exception($"Inventory record not found for Product {product.Id}");


                    if (inventoryProduct.Quantity < itemDto.Quantity)
                        throw new Exception($"Not enough stock for product {product.Id}");
                    inventoryProduct.Quantity -= itemDto.Quantity;
                    inventoryRepo.Update(inventoryProduct);

                }
                else if (itemDto.Unit == Unit.Piece || itemDto.Unit == Unit.Meter)
                {
                    var allShopProducts = await shopProductRepo.GetAllAsync();
                    var shopProduct = allShopProducts.FirstOrDefault(
                        sp => sp.ProductId == product.Id && sp.ShopId == dto.ShopId
                    );

                    if (shopProduct == null)
                        throw new Exception($"ShopProduct not found for Product {product.Id} in Shop {dto.ShopId}");

                    if (shopProduct.Quantity < itemDto.Quantity)
                        throw new Exception($"Not enough stock in Shop {dto.ShopId} for product {product.Id}");

                    shopProduct.Quantity -= itemDto.Quantity;
                    shopProductRepo.Update(shopProduct);

                }
                var invoiceItem = new InvoiceProduct
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.PricePerUnit
                };

                invoice.InvoiceProducts.Add(invoiceItem);
            }
            invoice.TotalPrice = invoice.InvoiceProducts.Sum(i => i.UnitPrice * i.Quantity);
            var invoiceRepo = unitOfWork.GetRepository<Invoice, int>();
            await invoiceRepo.AddAsync(invoice);
            await unitOfWork.SaveChangesAsync();
            return invoice.Id;
        }




        public async Task<bool> UpdateInvoiceAsync(int id, InvoiceUpdateDto dto)
        {
            var repo = unitOfWork.GetRepository<Invoice, int>();

            var invoice = await repo.GetAsync(id);
            if (invoice == null)
                return false;

            mapper.Map(dto, invoice);

          

            repo.Update(invoice);
           
            return true;
        }

     
        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var repo = unitOfWork.GetRepository<Invoice, int>();

            var invoice = await repo.GetAsync(id);
            if (invoice == null)
                return false;

            repo.Delete(invoice);
           
            return true;
        }

     
        public async Task<InvoiceResultDto?> GetInvoiceByIdAsync(int id)
        {
            var spec = new InvoiceWithProductsSpecification(id);
            var invoice = await unitOfWork.GetRepository<Invoice, int>().GetAsync(spec);

            return mapper.Map<InvoiceResultDto>(invoice);
        }

     
        public async Task<PaginationResponse<InvoiceResultDto>> GetInvoicesAsync(InvoiceSpecificationsParamters invoiceSpecsParams)
        {
            var spec = new InvoiceWithProductsSpecification(invoiceSpecsParams);
            var invoices = await unitOfWork.GetRepository<Invoice, int>().GetAllAsync(spec);

            var specCount = new InvoiceWithCountSpecification(invoiceSpecsParams);
            var count = await unitOfWork.GetRepository<Invoice, int>().CountAsync(specCount);

            var result = mapper.Map<IEnumerable<InvoiceResultDto>>(invoices);

            return new PaginationResponse<InvoiceResultDto>(
                pageIndex: invoiceSpecsParams.PageIndex,
                pageSize: invoiceSpecsParams.PageSize,
                totalCount: count,
                data: result
            );
        }
    }
}
