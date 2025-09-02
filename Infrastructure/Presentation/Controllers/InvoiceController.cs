using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.Invoice;
using Shared.DTOs.Product;
using Shared.ErrorModels;
using Shared.Response;
using Shared.SpecificationsParam.Invoice;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class InvoicesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet] //GET: /api/Invoices
      
        [ProducesResponseType<PaginationResponse<InvoiceResultDto>>(StatusCodes.Status200OK, Type = typeof(PaginationResponse<InvoiceResultDto>))]
        [ProducesResponseType<PaginationResponse<InvoiceResultDto>>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<InvoiceResultDto>>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<InvoiceResultDto>>> GetAll([FromQuery] InvoiceSpecificationsParamters invoiceSpecsParams)
        {
            var result = await serviceManager.InvoiceService.GetInvoicesAsync(invoiceSpecsParams);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
   
        [ProducesResponseType(typeof(InvoiceResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InvoiceResultDto>> GetById(int id)
        {
            var result = await serviceManager.InvoiceService.GetInvoiceByIdAsync(id);
            return result is null
                ? NotFound(new ErrorDetails { StatusCode = 404, ErrorMessage = "Invoice not found" })
                : Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(string userid, [FromBody] InvoiceCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User not authenticated");

            var userId = int.Parse(userIdClaim.Value);

            var newId = await serviceManager.InvoiceService.AddInvoiceAsync(userid, dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await serviceManager.InvoiceService.DeleteInvoiceAsync(id);

            if (!deleted)
                return NotFound(new ErrorDetails { StatusCode = 404, ErrorMessage = "Invoice not found" });

            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(InvoiceResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            var updatedInvoice = await serviceManager.InvoiceService.UpdateInvoiceAsync(id, dto);

            if (updatedInvoice == null)
                return NotFound(new ErrorDetails
                {
                    StatusCode = 404,
                    ErrorMessage = "Invoice not found"
                });


            return CreatedAtAction(
       nameof(GetById),                   
       new { id = updatedInvoice.Id },    
       updatedInvoice                     
   );
        }





    }
}
