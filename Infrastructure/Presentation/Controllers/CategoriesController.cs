using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.Category;
using Shared.DTOs.Product;
using Shared.ErrorModels;
using Shared.Response;
using Shared.SpecificationsParam.Category;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    [Authorize]
    public class CategoriesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet] //GET: /api/categories
        [ProducesResponseType<PaginationResponse<CategoryResultDto>>(StatusCodes.Status200OK, Type =  typeof(PaginationResponse<CategoryResultDto>))]
        [ProducesResponseType<PaginationResponse<CategoryResultDto>>(StatusCodes.Status500InternalServerError, Type =  typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<CategoryResultDto>>(StatusCodes.Status400BadRequest, Type =  typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<CategoryResultDto>>> GetAll([FromQuery] CategorySpecificationsParameters categorySpecsParams)
        {
            var result = await serviceManager.CategoryService.GetCategoriesAsync(categorySpecsParams);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType<CategoryResultDto>(StatusCodes.Status200OK, Type = typeof(CategoryResultDto))]
        [ProducesResponseType<CategoryResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<CategoryResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType<CategoryResultDto>(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<CategoryResultDto>> GetById(int id)
        {
            var result = await serviceManager.CategoryService.GetCategoryByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = await serviceManager.CategoryService.AddCategoryAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.CategoryService.UpdateCategoryAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await serviceManager.CategoryService.DeleteCategoryAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
