using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route(template:"api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        // sort : nameasc
        // sort : namedesc
        // sort : priecasc
        // sort : priecdesc
        public async Task<IActionResult> GetAll(int? categoryId, string? sort, int pageIndex = 1, int pageSize = 5)
        {
            var result = await serviceManager.ProductService.GetProductsAsync(categoryId, sort, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = await serviceManager.ProductService.AddProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.ProductService.UpdateProductAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await serviceManager.ProductService.DeleteProductAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
