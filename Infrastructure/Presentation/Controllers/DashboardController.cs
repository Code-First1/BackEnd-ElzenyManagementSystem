using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController(IDashboardService dashboardService):ControllerBase
    {
        [HttpGet("total-products")]
        public async Task<IActionResult> GetTotalProducts()
        {
            var totalProducts = await dashboardService.GetTotalProductsAsync();
            return Ok(totalProducts);
        }
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue([FromQuery] int days)
        {
            if (days <= 0)
            {
                return BadRequest("Days must be greater than zero.");
            }
            var revenue = await dashboardService.GetRevenueAsync(days);
            return Ok(revenue);
        }
        [HttpGet("low-stock-products")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var lowStockProducts = await dashboardService.GetLowStockProductsAsync();
            return Ok(lowStockProducts);
        }


    }
}
