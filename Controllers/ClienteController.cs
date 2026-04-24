using apiSimex.Models;
using ApiSimexCsharp.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ApiSimexCsharp.Models;

namespace apiSimex.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ClientController : ControllerBase
    {
        private readonly Simex01Context _context;

        public ClientController(Simex01Context context)
        {
            _context = context;
        }

        [HttpGet("client/dashboard/{userId}")]
        public async Task<ActionResult<ClientDashboardDto>> GetClientDashboard(int userId)
        {
            try
            {
                var nombreUsuario = await _context.Usuaris
                    .Where(u => u.Id == userId)
                    .Select(u => u.Nom)
                    .FirstOrDefaultAsync();

                if (nombreUsuario == null)
                    return NotFound("Usuario no encontrado");

                var active = await _context.Ofertes
                    .CountAsync(o => o.ClientId == userId && o.EstatOfertaId == 1);

                var pending = await _context.Ofertes
                    .CountAsync(o => o.ClientId == userId && o.EstatOfertaId == 2);

                return Ok(new ClientDashboardDto
                {
                    user_name = nombreUsuario,
                    active_count = active,
                    pending_count = pending
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
