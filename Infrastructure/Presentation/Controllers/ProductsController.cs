using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.Product;
using Shared.ErrorModels;
using Shared.Response;
using Shared.SpecificationsParam.Product;
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
        // sort : nameasc
        // sort : namedesc
        // sort : priecasc
        // sort : priecdesc

        [HttpGet] //GET: /api/Products
        [Authorize]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status200OK, Type =  typeof(PaginationResponse<ProductResultDto>))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status500InternalServerError, Type =  typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<ProductResultDto>>> GetAll([FromQuery]ProductSpecificationsParamters productSpecsParams)
        {
            
            var result = await serviceManager.ProductService.GetProductsAsync(productSpecsParams);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType<ProductResultDto>(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType<ProductResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<ProductResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType<ProductResultDto>(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [ProducesResponseType<AddProductDto>(StatusCodes.Status200OK, Type = typeof(AddProductDto))]

        public async Task<ActionResult<AddProductDto>> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await serviceManager.ProductService.AddProductAsync(dto);
            return result is null ? NotFound() : Ok(result); 
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
