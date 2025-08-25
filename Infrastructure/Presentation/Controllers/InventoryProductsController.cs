using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.InventoryProduct;
using Shared.ErrorModels;
using Shared.Response;
using Shared.SpecificationsParam.InventoryProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryProductsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType<PaginationResponse<InventoryProductResultDto>>(StatusCodes.Status200OK, Type = typeof(PaginationResponse<InventoryProductResultDto>))]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationResponse<InventoryProductResultDto>>> GetAll([FromQuery] InventoryProductSpecificationsParams specParams)
        {
            var result = await serviceManager.InventoryProductService.GetAllAsync(specParams);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType<InventoryProductResultDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InventoryProductResultDto>> GetById(int id)
        {
            var result = await serviceManager.InventoryProductService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.InventoryProductService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

    }
}
