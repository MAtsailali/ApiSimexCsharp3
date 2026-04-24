using Microsoft.AspNetCore.Mvc;
using apiSimex.Models;

namespace apiSimex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        /// Dashboard del usuario cliente
        [HttpGet("{userId}")]
        public IActionResult GetDashboard(int userId)
        {
            var response = new DashboardResponse
            {
                ActiveCount = 12,
                PendingCount = 48,
                RecentActivities = new List<RecentActivityDto>
                {
                    new RecentActivityDto
                    {
                        Title = "Pedido #1001",
                        Description = "Barcelona",
                        Status = "ENTREGADO",
                        Date = "2026-04-23",
                        Icon = "shipment"
                    },
                    new RecentActivityDto
                    {
                        Title = "Pedido #1002",
                        Description = "Madrid",
                        Status = "PENDIENTE",
                        Date = "2026-04-22",
                        Icon = "warning"
                    }
                }
            };

            return Ok(response);
        }
    }
}