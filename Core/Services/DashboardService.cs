using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DashboardService(IUnitOfWork unitOfWork, IMapper mapper) : IDashboardService
    {

        public async Task<decimal> GetRevenueAsync(int days)
        {
            var invoiceRepo = unitOfWork.GetRepository<Invoice, int>();
            var today = DateTime.Today;

            DateTime startDate;
            DateTime endDate;

            if (days == 1)
            {
                
                startDate = today;
                endDate = today.AddDays(1);
            }
            else if (days == 7)
            {
                
                int daysSinceSunday = (int)today.DayOfWeek;
                startDate = today.AddDays(-daysSinceSunday);
                endDate = today.AddDays(1);
            }
            else
            {
                
                startDate = today.AddDays(-days);
                endDate = today.AddDays(1);
            }

            var spec = new BaseSpecifications<Invoice, int>(
                i => i.CreateAt >= startDate && i.CreateAt < endDate
            );

            var invoices = await invoiceRepo.GetAllAsync(spec);

            
            if (invoices == null || !invoices.Any())
                return 0;

            return invoices.Sum(i => i.TotalPrice);
        }


        public async Task<int> GetLowStockProductsAsync()
        {
            var inventoryProductRepo = unitOfWork.GetRepository<InventoryProduct, int>();
            var product = await inventoryProductRepo.GetAllAsync();
            var lowStockProducts = product.Where(p => p.Quantity<=p.MinimumQuantity).ToList();
            return lowStockProducts.Count();
        }

        public async Task<int> GetTotalProductsAsync()
        {
            var productRepo = unitOfWork.GetRepository<Product, int>();
            var product =await productRepo.GetAllAsync();
            return product.Count();
        }

  
    }
}
