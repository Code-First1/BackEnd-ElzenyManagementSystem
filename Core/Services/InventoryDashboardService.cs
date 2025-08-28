using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.InventoryDashboard;
using Shared.DTOs.InventoryDashboard;
using Shared.DTOs.InventoryProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class InventoryDashboardService(IUnitOfWork unitOfWork, IMapper mapper) : IInventoryDashboardService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<InventoryDashboardCountDto> GetProductsCountAsync()
        {
            var repo = _unitOfWork.GetRepository<InventoryProduct, int>();

            var allCount = await repo.CountAsync(new AllProductsSpecifications());
            var goodCount = await repo.CountAsync(new GoodProductsSpecification());
            var criticalCount = await repo.CountAsync(new CriticalProductsSpecification());
            var emptyCount = await repo.CountAsync(new EmptyProductsSpecification());

            return new InventoryDashboardCountDto
            {
                TotalProductsCount = allCount,
                GoodProductsCount = goodCount,
                CriticalProductsCount = criticalCount,
                EmptyProductsCount = emptyCount
            };
        }

        // -------- Lists --------
        public async Task<IEnumerable<InventoryProductResultDto>> GetGoodProductsAsync()
        {
            var spec = new GoodProductsSpecification();
            var products = await _unitOfWork.GetRepository<InventoryProduct, int>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<InventoryProductResultDto>>(products);
        }

        public async Task<IEnumerable<InventoryProductResultDto>> GetCriticalProductsAsync()
        {
            var spec = new CriticalProductsSpecification();
            var products = await _unitOfWork.GetRepository<InventoryProduct, int>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<InventoryProductResultDto>>(products);
        }

        public async Task<IEnumerable<InventoryProductResultDto>> GetEmptyProductsAsync()
        {
            var spec = new EmptyProductsSpecification();
            var products = await _unitOfWork.GetRepository<InventoryProduct, int>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<InventoryProductResultDto>>(products);
        }
    }
}
