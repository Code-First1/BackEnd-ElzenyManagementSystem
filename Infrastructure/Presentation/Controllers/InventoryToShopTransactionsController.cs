using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.InventoryToShopTransation;
using Shared.DTOs.Product;
using Shared.DTOs.TransationFromInventoryToShopInProduct;
using Shared.ErrorModels;
using Shared.Response;
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
    public class InventoryToShopTransactionsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<TransactionPerProductResultDto>(StatusCodes.Status200OK, Type = typeof(TransactionPerProductResultDto))]
        [ProducesResponseType<TransactionPerProductResultDto>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<TransactionPerProductResultDto>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionPerProductCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await serviceManager.TransactionPerProductService.CreateTransactionAsync(dto);
            return Ok(result);
        }

        
    }
}
