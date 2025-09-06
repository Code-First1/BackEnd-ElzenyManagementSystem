using AutoMapper;
using Domain.Contracts;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Identity;
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

        public async Task<InvoiceCreateResultDto> AddInvoiceAsync(string userName, [FromBody] InvoiceCreateDto dto)
        {
            var invoice = new Invoice
            {
                ShopId = dto.ShopId,
                UserName = userName,
                CreateAt = DateTime.UtcNow,
                InvoiceProducts = new List<InvoiceProduct>()
            };

            var productRepo = unitOfWork.GetRepository<Product, int>();
            var inventoryRepo = unitOfWork.GetRepository<InventoryProduct, int>();
            var shopProductRepo = unitOfWork.GetRepository<ShopProduct, int>();
            

            var allInventory = await inventoryRepo.GetAllAsync();
            var shopProducts = await shopProductRepo.GetAllAsync();

            decimal sum = 0;
            int transactionCount = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = await productRepo.GetAsync(itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product with Id {itemDto.ProductId} not found");

                var inventoryProduct = allInventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (inventoryProduct == null)
                    throw new Exception($"Inventory record not found for Product {product.Id}");

                var shopProduct = shopProducts.FirstOrDefault(sp => sp.ProductId == product.Id && sp.ShopId == dto.ShopId);
                if (shopProduct == null)
                    throw new Exception($"ShopProduct not found for Product {product.Id} in Shop {dto.ShopId}");

                if (itemDto.Quantity <= 0)
                    throw new Exception("Quantity must be greater than 0");


                if (shopProduct.Quantity < itemDto.Quantity)
                {

                    int availableInShop = shopProduct.Quantity;


                    itemDto.Quantity -= availableInShop;
                    shopProduct.Quantity = 0;
                    if (product.QuantityForOrigin <= 0)
                    {
                        throw new Exception("Quantity must be greater than 0 in database");
                    }


                    int neededFromInventory = (int)Math.Ceiling((double)itemDto.Quantity / product.QuantityForOrigin);

                    if (inventoryProduct.Quantity < neededFromInventory)
                        throw new Exception($"Not enough stock in Inventory for product {product.Id}");


                    inventoryProduct.Quantity -= neededFromInventory;


                    shopProduct.Quantity += neededFromInventory * product.QuantityForOrigin;
                   

                    if (shopProduct.Quantity < itemDto.Quantity)
                        throw new Exception($"Still not enough stock in Shop after refill for product {product.Id}");

                    shopProduct.Quantity -= itemDto.Quantity;
                    transactionCount = shopProduct.Quantity;
                }
                else
                {

                    shopProduct.Quantity -= itemDto.Quantity;
                }


                shopProductRepo.Update(shopProduct);
                inventoryRepo.Update(inventoryProduct);


                decimal unitPrice = itemDto.Typing ? product.PrieceForWholeSale : product.PriceForRetail;

                sum += itemDto.Quantity * unitPrice;

                var invoiceItem = new InvoiceProduct
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                    TransferCount=transactionCount
                };

                invoice.InvoiceProducts.Add(invoiceItem);
            }

            invoice.TotalPrice = sum;
            invoice.UserName = userName;
            var invoiceRepo = unitOfWork.GetRepository<Invoice, int>();
            await invoiceRepo.AddAsync(invoice);
            await unitOfWork.SaveChangesAsync();

            return new InvoiceCreateResultDto
            {
                Id = invoice.Id,
                TransferCount = transactionCount
            };
        }



        public async Task<InvoiceResultDto?> UpdateInvoiceAsync(int id, InvoiceUpdateDto dto)
        {
            var invoiceRepo = unitOfWork.GetRepository<Invoice, int>();


            var invoice = await invoiceRepo.GetAsync(id);
            if (invoice == null)
                return null;




            if (invoice.InvoiceProducts != null)
                invoice.TotalPrice = invoice.InvoiceProducts.Sum(p => p.Quantity * p.UnitPrice);


            invoiceRepo.Update(invoice);

            await unitOfWork.SaveChangesAsync();


            return mapper.Map<InvoiceResultDto>(invoice);
        }


        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoiceRepo = unitOfWork.GetRepository<Invoice, int>();
            var invoiceProductRepo = unitOfWork.GetRepository<InvoiceProduct, int>();

            var invoice = await invoiceRepo.GetAsync(id);
            if (invoice == null)
                return false;


            var allInvoiceProducts = await invoiceProductRepo.GetAllAsync();
            var invoiceProducts = allInvoiceProducts.Where(ip => ip.InvoiceId == id);

            foreach (var item in invoiceProducts)
            {
                invoiceProductRepo.Delete(item);
            }

            invoiceRepo.Delete(invoice);

            await unitOfWork.SaveChangesAsync();

            return true;
        }



        public async Task<InvoiceResultDto?> GetInvoiceByIdAsync(int id)
        {
            var spec = new InvoiceWithProductsSpecification(id);
            var invoice = await unitOfWork.GetRepository<Invoice, int>().GetAsync(spec);

            if (invoice == null)
                return null;

            var productRepo = unitOfWork.GetRepository<Product, int>();
            var allProducts = await productRepo.GetAllAsync();

            var invoiceProductRepo = unitOfWork.GetRepository<InvoiceProduct, int>();
            var allInvoiceProducts = await invoiceProductRepo.GetAllAsync();

            var result = mapper.Map<InvoiceResultDto>(invoice);

            decimal total = 0;

            foreach (var item in result.InvoiceProduct)
            {
                
                var product = allProducts.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    item.ProductName = product.Name;
                }

            
                var invProd = allInvoiceProducts
                    .FirstOrDefault(ip => ip.ProductId == item.ProductId && ip.InvoiceId == result.Id);

                if (invProd != null)
                {
                    item.pricePerUnit = invProd.UnitPrice;
                    total += invProd.UnitPrice * item.Quantity;
                }
            }

            result.Total = total;

            return result;
        }



        public async Task<InvoicePaginationResponse<InvoiceResultDto>> GetInvoicesAsync(InvoiceSpecificationsParamters invoiceSpecsParams)
        {
       
            var spec = new InvoiceWithProductsSpecification(invoiceSpecsParams);
            var invoices = await unitOfWork.GetRepository<Invoice, int>().GetAllAsync(spec);

            if (!invoices.Any())
            {
                return new InvoicePaginationResponse<InvoiceResultDto>(
                    pageIndex: invoiceSpecsParams.PageIndex,
                    pageSize: invoiceSpecsParams.PageSize,
                    totalCount: 0,
                    data: new List<InvoiceResultDto>(),
                    grandTotal: 0
                );
            }


            var productRepo = unitOfWork.GetRepository<Product, int>();
            var allProducts = await productRepo.GetAllAsync();

            
            var invoiceProductRepo = unitOfWork.GetRepository<InvoiceProduct, int>();
            var allInvoiceProducts = await invoiceProductRepo.GetAllAsync();

          
            var result = mapper.Map<IEnumerable<InvoiceResultDto>>(invoices);

        
            foreach (var invoiceDto in result)
            {
                decimal total = 0;

                foreach (var item in invoiceDto.InvoiceProduct)
                {
                
                    var product = allProducts.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        item.ProductName = product.Name;
                    }

               
                    var invProd = allInvoiceProducts
                        .FirstOrDefault(ip => ip.ProductId == item.ProductId && ip.InvoiceId == invoiceDto.Id);

                    if (invProd != null)
                    {
                        item.pricePerUnit = invProd.UnitPrice;
                        total += invProd.UnitPrice * item.Quantity;
                    }
                }

                invoiceDto.Total = total;
            }

            var grandTotal = result.Sum(r => r.Total);
            var specCount = new InvoiceWithCountSpecification(invoiceSpecsParams);
            var count = await unitOfWork.GetRepository<Invoice, int>().CountAsync(specCount);
            return new InvoicePaginationResponse<InvoiceResultDto>(
                  pageIndex: invoiceSpecsParams.PageIndex,
                  pageSize: invoiceSpecsParams.PageSize,
                  totalCount: count,
                   grandTotal: grandTotal,
                  data: result
                 
                  );



        }
    }
    }
