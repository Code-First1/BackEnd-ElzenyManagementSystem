using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.Category;
using Shared.DTOs.SubCategor;
using Shared.ErrorModels;
using Shared.SpecificationsParam.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route(template:"api/[controller]")]
    public class SubCategoriesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet] //GET: /api/subcategories
        [ProducesResponseType<IEnumerable<SubCategoryResultDto>>(StatusCodes.Status200OK, Type =  typeof(IEnumerable<SubCategoryResultDto>))]
        [ProducesResponseType<IEnumerable<SubCategoryResultDto>>(StatusCodes.Status500InternalServerError, Type =  typeof(ErrorDetails))]
        [ProducesResponseType<IEnumerable<SubCategoryResultDto>>(StatusCodes.Status400BadRequest, Type =  typeof(ErrorDetails))]
        public async Task<ActionResult<IEnumerable<SubCategoryResultDto>>> GetSubCategoriesAsync([FromQuery] SubCategorySpecificationsParameters subCategorySpecsParams)
        {
            var result = await serviceManager.SubCategoryService.GetAllSubCategoriesAsync(subCategorySpecsParams);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType<SubCategoryResultDto>(StatusCodes.Status200OK, Type = typeof(SubCategoryResultDto))]
        [ProducesResponseType<SubCategoryResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<SubCategoryResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType<SubCategoryResultDto>(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<SubCategoryResultDto>> GetById(int id)
        {
            var result = await serviceManager.SubCategoryService.GetSubCategoryByIdAsync(id);
            return result is null ? NotFound() : Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = await serviceManager.SubCategoryService.AddSubCategoryAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId.ToString() }, new { id = newId });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubCategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.SubCategoryService.UpdateSubCategoryAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await serviceManager.SubCategoryService.DeleteSubCategoryAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
