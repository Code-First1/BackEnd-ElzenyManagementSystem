using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.ShopProduct;
using Shared.ErrorModels;
using Shared.Response;
using Shared.SpecificationsParam.ShopProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShopProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // GET: api/ShopProducts
        [HttpGet]
        [ProducesResponseType<PaginationResponse<ShopProductResultDto>>(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ShopProductResultDto>))]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginationResponse<ShopProductResultDto>>> GetAll([FromQuery] ShopProductSpecificationsParams specParams)
        {
            var result = await serviceManager.ShopProductService.GetAllAsync(specParams);
            return Ok(result);
        }

        // GET: api/ShopProducts/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType<ShopProductResultDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShopProductResultDto>> GetById(int id)
        {
            var result = await serviceManager.ShopProductService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }


        // PUT: api/ShopProducts/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShopProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.ShopProductService.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

    }
}
