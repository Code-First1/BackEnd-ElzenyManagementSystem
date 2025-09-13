using AutoMapper;
using Domain.Contracts;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Specifications.Invoices;
using Shared.DTOs.Invoice;
using Shared.Response;
using Shared.SpecificationsParam.Invoice;


namespace Services
{
    public class InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IInvoiceService
    {

        public async Task<InvoiceCreateResultDto> AddInvoiceAsync(string userName, [FromBody] InvoiceCreateDto dto)
        {
            var invoice = new Invoice
            {
                ShopId = dto.ShopId,
                UserName = userName,
                CreateAt = GetEgyptTime(),
                InvoiceProducts = new List<InvoiceProduct>()
            };

            var productRepo = unitOfWork.GetRepository<Product, int>();
            var inventoryRepo = unitOfWork.GetRepository<InventoryProduct, int>();
            var shopProductRepo = unitOfWork.GetRepository<ShopProduct, int>();
            var invoiceRepo = unitOfWork.GetRepository<Invoice, int>();
            var allInventory = await inventoryRepo.GetAllAsync();
            var shopProducts = await shopProductRepo.GetAllAsync();

            decimal sum = 0;
            var resultItems = new List<InvoiceDerivedCreateDto>();

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

                int transactionCount = 0;

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
                    transactionCount = neededFromInventory;
                }
                else
                {
                    shopProduct.Quantity -= itemDto.Quantity;
                    transactionCount = 0;
                }

                shopProductRepo.Update(shopProduct);
                inventoryRepo.Update(inventoryProduct);

                decimal unitPrice = itemDto.Typing ? product.PrieceForWholeSale : product.PriceForRetail;
                sum += itemDto.Quantity * unitPrice;

                // Add invoice product to invoice
                var invoiceItem = new InvoiceProduct
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                    TransferCount = transactionCount
                };
                invoice.InvoiceProducts.Add(invoiceItem);

                // Add to result DTO
                resultItems.Add(new InvoiceDerivedCreateDto
                {
                    productId = product.Id,
                    productName = product.Name,
                    transferCount = transactionCount
                });
            }

            invoice.TotalPrice = sum;
            invoice.UserName = userName;
            var today = DateTime.UtcNow.Date;
            var lastInvoiceToday = (await invoiceRepo.GetAllAsync())
                                    .Where(i => i.CreateAt.Date == today)
                                    .OrderByDescending(i => i.number)
                                    .FirstOrDefault();

            int newNumber = lastInvoiceToday != null ? lastInvoiceToday.number + 1 : 1;

            invoice.number = newNumber;
            await invoiceRepo.AddAsync(invoice);
            await unitOfWork.SaveChangesAsync();

            return new InvoiceCreateResultDto
            {
                Id = invoice.Id,
                Items = resultItems
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
            // Get paginated invoices for display
            var spec = new InvoiceWithProductsSpecification(invoiceSpecsParams);
            var paginatedInvoices = await unitOfWork.GetRepository<Invoice, int>().GetAllAsync(spec);

            // Get all products and invoice products for calculations
            var productRepo = unitOfWork.GetRepository<Product, int>();
            var allProducts = await productRepo.GetAllAsync();
            var invoiceProductRepo = unitOfWork.GetRepository<InvoiceProduct, int>();
            var allInvoiceProducts = await invoiceProductRepo.GetAllAsync();

            // Calculate grand total for ALL invoices (without pagination, but with filters)
            var specForGrandTotal = new InvoiceWithProductsSpecification(new InvoiceSpecificationsParamters
            {
                Search = invoiceSpecsParams.Search,
                DisplayName = invoiceSpecsParams.DisplayName,
                CreateAt = invoiceSpecsParams.CreateAt,
                PageIndex = 1,
                PageSize = int.MaxValue
            });

            var allInvoicesForTotal = await unitOfWork.GetRepository<Invoice, int>().GetAllAsync(specForGrandTotal);
            decimal totalGrandAmount = await CalculateGrandTotal(allInvoicesForTotal, allProducts, allInvoiceProducts);

            if (!paginatedInvoices.Any())
            {
                return new InvoicePaginationResponse<InvoiceResultDto>(
                    pageIndex: invoiceSpecsParams.PageIndex,
                    pageSize: invoiceSpecsParams.PageSize,
                    totalCount: 0,
                    data: new List<InvoiceResultDto>(),
                    grandTotal: totalGrandAmount
                );
            }

            // Map paginated results
            var result = mapper.Map<IEnumerable<InvoiceResultDto>>(paginatedInvoices).ToList();

            // Calculate totals for paginated results
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

            // Get total count for pagination
            var specCount = new InvoiceWithCountSpecification(invoiceSpecsParams);
            var count = await unitOfWork.GetRepository<Invoice, int>().CountAsync(specCount);

            return new InvoicePaginationResponse<InvoiceResultDto>(
                pageIndex: invoiceSpecsParams.PageIndex,
                pageSize: invoiceSpecsParams.PageSize,
                totalCount: count,
                data: result,
                grandTotal: totalGrandAmount
            );
        }

        // Helper method to calculate grand total
        private async Task<decimal> CalculateGrandTotal(
            IEnumerable<Invoice> invoices,
            IEnumerable<Product> products = null,
            IEnumerable<InvoiceProduct> invoiceProducts = null)
        {
            if (!invoices.Any())
                return 0;

            // Load products and invoice products if not provided
            if (products == null)
            {
                var productRepo = unitOfWork.GetRepository<Product, int>();
                products = await productRepo.GetAllAsync();
            }

            if (invoiceProducts == null)
            {
                var invoiceProductRepo = unitOfWork.GetRepository<InvoiceProduct, int>();
                invoiceProducts = await invoiceProductRepo.GetAllAsync();
            }

            decimal grandTotal = 0;

            foreach (var invoice in invoices)
            {
                decimal invoiceTotal = 0;
                var invoiceProductsForThisInvoice = invoiceProducts.Where(ip => ip.InvoiceId == invoice.Id);

                foreach (var invProd in invoiceProductsForThisInvoice)
                {
                    invoiceTotal += invProd.UnitPrice * invProd.Quantity;
                }

                grandTotal += invoiceTotal;
            }

            return grandTotal;
        }

        
        private DateTime GetEgyptTime()
        {
            var ctx = httpContextAccessor.HttpContext;
            if (ctx != null && ctx.Items["EgyptTime"] is DateTime egyptTime)
                return egyptTime;


            var tz = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
        }

    }
}
