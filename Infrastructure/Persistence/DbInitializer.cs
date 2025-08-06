using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ElzenyDbContext _context;
        private readonly ElzenyIdentityDbContext _identityDbContet;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(
            ElzenyDbContext context,
            ElzenyIdentityDbContext identityDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _identityDbContet = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            try
            {
                //Create Db If it doesn't Exists && Apply To Any Pending Migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }

                //Data Seeding
                //Seeding Categories From Json Files
                if (!_context.Categories.Any())
                {
                    //1. Read All Data From categories.json as String
                    var categoriesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\categories.json");

                    //2. Transform String To C# Object [List<Category>]
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                    //3. Add List<Category> To Database
                    if (categories is not null && categories.Any())
                    {
                        await _context.Categories.AddRangeAsync(categories);
                        await _context.SaveChangesAsync();

                    }
                }

                //Seeding Products From Json Files
                if (!_context.Products.Any())
                {
                    //1. Read All Data From products.json as String
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Seeding\products.json");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    };

                    //2. Transform String To C# Object [List<Product>]
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData, options);

                    //3. Add List<Product> To Database
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();

                    }
                }
            }
            catch(Exception) 
            {
                throw;
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // Create Databse If it doesnt Exxists && Apply To Any Pending Migrations
            if(_identityDbContet.Database.GetPendingMigrations().Any())
            {
                await _identityDbContet.Database.MigrateAsync();
            }

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(role: new IdentityRole() 
                {
                    Name = "Admin"
                });
                await _roleManager.CreateAsync(role: new IdentityRole() 
                {
                    Name = "Seller"
                });
            }

            // Seeding
            if(!_userManager.Users.Any())
            {
                var adminUser = new AppUser()
                {
                    DisplayName = "Elzeny",
                    UserName = "MostafaElzeny"
                };
                var sellerUser = new AppUser()
                {
                    DisplayName = "Abdo",
                    UserName = "AbdoAli"
                };
                var result1 = await _userManager.CreateAsync(adminUser, "P@ssW0rd");
                if (!result1.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result1.Errors.Select(e => e.Description)));
                }

                var result2 = await _userManager.CreateAsync(sellerUser, "P@ssW0rd");
                if (!result2.Succeeded)
                {
                    throw new Exception("Failed to create seller user: " + string.Join(", ", result2.Errors.Select(e => e.Description)));
                }

                await _userManager.AddToRoleAsync(adminUser, role: "Admin");
                await _userManager.AddToRoleAsync(sellerUser, role: "Seller");
            }
        }
    }
}
// ..\Infrastructure\Persistence\Seeding\categories.json