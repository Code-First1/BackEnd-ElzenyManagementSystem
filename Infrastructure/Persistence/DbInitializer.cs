using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
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
        public DbInitializer(ElzenyDbContext context)
        {
            _context = context;
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
    }
}
// ..\Infrastructure\Persistence\Seeding\categories.json