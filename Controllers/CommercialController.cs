namespace ApiSimexCsharp.Controllers;
using ApiSimexCsharp.DTO;
using ApiSimexCsharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/commercial")]
public class CommercialController : ControllerBase
{
    private readonly Simex01Context _context;

    public CommercialController(Simex01Context context)
    {
        _context = context;
    }

    [HttpGet("dashboard/{userId}")]
    public async Task<ActionResult<CommercialDashboardDto>> GetDashboard(int userId)
    {
        try
        {
            // 1. Buscamos el nombre del comercial en la tabla 'Usuaris'
            var nombreUsuario = await _context.Usuaris
                .Where(u => u.Id == userId)
                .Select(u => u.Nom)
                .FirstOrDefaultAsync();

            if (nombreUsuario == null) return NotFound("Usuario no encontrado");

            // 2. Consultas de conteo en la tabla 'Ofertes'
            // Filtramos por AgentComercialId (que es el campo en tu clase Oferte)

            var pending = await _context.Ofertes
                .CountAsync(o => o.AgentComercialId == userId && o.EstatOfertaId == 2); // 2 = Pendent

            var active = await _context.Ofertes
                .CountAsync(o => o.AgentComercialId == userId && o.EstatOfertaId == 1); // 1 = Activa

            var rejected = await _context.Ofertes
                .CountAsync(o => o.AgentComercialId == userId && o.EstatOfertaId == 4); // 4 = Cancel·lada

            // 3. Devolvemos el DTO para Android
            return Ok(new CommercialDashboardDto
            {
                user_name = nombreUsuario,
                pending_count = pending,
                active_ops_count = active,
                rejected_count = rejected
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


    [HttpGet("ofertes/sent/{userId}")]
    public async Task<ActionResult> GetSentQuotes(int userId)
    {
        try
        {
            var sentQuotes = await _context.Ofertes
                .Where(o => o.AgentComercialId == userId && o.EstatOfertaId == 2)
                .Include(o => o.PortOrigen).Include(o => o.PortDesti)
                .Include(o => o.AeroportOrigen).Include(o => o.AeroportDesti)
                .OrderByDescending(o => o.DataCreacio)
                .Select(o => new {
                    id = o.Id,
                    price = string.IsNullOrEmpty(o.Valor) ? "Consultar" : o.Valor,
                    route = o.PortOrigen != null && o.PortDesti != null ? $"{o.PortOrigen.Nom} / {o.PortDesti.Nom}" : (o.AeroportOrigen != null ? $"{o.AeroportOrigen.Nom} / {o.AeroportDesti.Nom}" : "Ruta pendiente"),
                    description = o.Concepto ?? "Envío de mercancía",
                    rejection_reason = "Pendiente de respuesta",
                    transport_type_id = o.TipusTransportId
                }).ToListAsync();

            return Ok(sentQuotes);
        }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }


    [HttpGet("ofertes/accepted/{userId}")]
    public async Task<ActionResult> GetAcceptedQuotes(int userId)
    {
        try
        {
            var acceptedQuotes = await _context.Ofertes
                .Where(o => o.AgentComercialId == userId && o.EstatOfertaId == 1)
                .Include(o => o.PortOrigen).Include(o => o.PortDesti)
                .Include(o => o.AeroportOrigen).Include(o => o.AeroportDesti)
                .OrderByDescending(o => o.DataCreacio)
                .Select(o => new {
                    id = o.Id,
                    price = string.IsNullOrEmpty(o.Valor) ? "Consultar" : o.Valor,
                    route = o.PortOrigen != null && o.PortDesti != null ? $"{o.PortOrigen.Nom} / {o.PortDesti.Nom}" : (o.AeroportOrigen != null ? $"{o.AeroportOrigen.Nom} / {o.AeroportDesti.Nom}" : "Ruta pendiente"),
                    description = o.Concepto ?? "Envío de mercancía",
                    rejection_reason = "Operación Activa",
                    transport_type_id = o.TipusTransportId
                }).ToListAsync();

            return Ok(acceptedQuotes);
        }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }


    [HttpGet("ofertes/rejected/{userId}")]
    public async Task<ActionResult> GetRejectedQuotes(int userId)
    {
        try
        {
            var rejectedQuotes = await _context.Ofertes
                .Where(o => o.AgentComercialId == userId && o.EstatOfertaId == 4)
                .Include(o => o.PortOrigen).Include(o => o.PortDesti)
                .Include(o => o.AeroportOrigen).Include(o => o.AeroportDesti)
                .OrderByDescending(o => o.DataCreacio)
                .Select(o => new {
                    id = o.Id,
                    price = string.IsNullOrEmpty(o.Valor) ? "Consultar" : o.Valor,
                    route = o.PortOrigen != null && o.PortDesti != null ? $"{o.PortOrigen.Nom} / {o.PortDesti.Nom}" : (o.AeroportOrigen != null ? $"{o.AeroportOrigen.Nom} / {o.AeroportDesti.Nom}" : "Ruta pendiente"),
                    description = o.Concepto ?? "Envío de mercancía",
                    rejection_reason = o.RaoRebuig ?? "Sin motivo especificado",
                    transport_type_id = o.TipusTransportId
                }).ToListAsync();

            return Ok(rejectedQuotes);
        }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }
}