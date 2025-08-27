using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.InventoryToShopTransation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryToShopTransactionsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] InventoryToShopTransactionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transactionId = await serviceManager.InventoryToShopTransactionService.CreateTransactionAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = transactionId }, null);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // ممكن تعمل DTO للـ Transaction مع Items
            return Ok($"Transaction {id} details here ...");
        }
    }
}
