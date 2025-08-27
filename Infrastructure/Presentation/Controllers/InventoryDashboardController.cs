using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs.InventoryDashboard;
using Shared.DTOs.InventoryProduct;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryDashboardController(IServiceManager serviceManager) : ControllerBase
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        // ---------- Counts ----------

        [HttpGet("counts")]
        [Authorize]
        [ProducesResponseType<InventoryDashboardCountDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryDashboardCountDto>> GetCounts()
        {
            var result = await _serviceManager.InventoryDashboardService.GetProductsCountAsync();
            return Ok(result);
        }

        // ---------- Lists ----------

        [HttpGet("good")]
        [Authorize]
        [ProducesResponseType<IEnumerable<InventoryProductResultDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryProductResultDto>>> GetGoodProducts()
        {
            var result = await _serviceManager.InventoryDashboardService.GetGoodProductsAsync();
            return Ok(result);
        }

        [HttpGet("critical")]
        [Authorize]
        [ProducesResponseType<IEnumerable<InventoryProductResultDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryProductResultDto>>> GetCriticalProducts()
        {
            var result = await _serviceManager.InventoryDashboardService.GetCriticalProductsAsync();
            return Ok(result);
        }

        [HttpGet("empty")]
        [Authorize]
        [ProducesResponseType<IEnumerable<InventoryProductResultDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryProductResultDto>>> GetEmptyProducts()
        {
            var result = await _serviceManager.InventoryDashboardService.GetEmptyProductsAsync();
            return Ok(result);
        }
    }
}
