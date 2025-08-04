using Domain.Models;
using Shared.DTOs.Category;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        //GetAllProduct
        Task<IEnumerable<ProductResultDto>> GetProductsAsync(int? categoryId, string? sort);
        //GetById
        Task<ProductResultDto?> GetProductByIdAsync(int id);

        Task<int> AddProductAsync(ProductCreateDto dto); 

        Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto);

        Task<bool> DeleteProductAsync(int id);

    }
}
