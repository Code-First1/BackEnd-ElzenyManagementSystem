using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
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
        IOptions<JwtOptions> options,
        RoleManager<IdentityRole> roleManager
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);
        public ICategoryService CategoryService { get; } = new CategoryService(unitOfWork, mapper);
        public ISubCategoryService SubCategoryService { get; } = new SubCategoryService(unitOfWork, mapper);
        public IInventoryProductService InventoryProductService { get; } = new InventoryProductService(unitOfWork, mapper);
        public IShopProductService ShopProductService { get; } = new ShopProductService(unitOfWork, mapper);

        public IAuthService AuthService { get; } = new AuthService(userManager, options, roleManager);

        
    }
}
