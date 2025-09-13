using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> options,
        RoleManager<IdentityRole> roleManager
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);
        public ICategoryService CategoryService { get; } = new CategoryService(unitOfWork, mapper);
        public ISubCategoryService SubCategoryService { get; } = new SubCategoryService(unitOfWork, mapper);
        public IInventoryProductService InventoryProductService { get; } = new InventoryProductService(unitOfWork, mapper);
        public IShopProductService ShopProductService { get; } = new ShopProductService(unitOfWork, mapper);

        public IInvoiceService InvoiceService { get; }= new InvoiceService(unitOfWork, mapper, httpContextAccessor);
        public ITransactionPerProductService TransactionPerProductService { get; } = new TransactionPerProductService(unitOfWork);
        public IInventoryToShopTransactionService InventoryToShopTransactionService { get; } = new InventoryToShopTransactionService(unitOfWork, mapper);
        public IInventoryDashboardService InventoryDashboardService { get;} = new InventoryDashboardService(unitOfWork, mapper);
        public IAuthService AuthService { get; } = new AuthService(userManager, options, roleManager);

        public IDashboardService DashboardService { get; } = new DashboardService(unitOfWork, mapper);
    }
}
