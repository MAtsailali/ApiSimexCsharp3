using apiSimex.Models;
using ApiSimexCsharp.DTO;
using ApiSimexCsharp.DTO.ApiSimexCsharp.DTO;
using ApiSimexCsharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("client/envios/activos/{userId}")]
        public async Task<ActionResult<IEnumerable<EnvioActivoDto>>> GetClientEnviosActivos(int userId)
        {
            try
            {
                // Definimos los estados que el cliente verá como "Mis Envíos"
                // 5: Aceptada (Pagada/Confirmada), 6: En Tránsito
                // Podrías añadir el 7 si quieres mostrar también los Recién Finalizados
                var estadosActivos = new List<int> { 5, 6 };

                var envios = await _context.Ofertes
                    .Where(o => o.ClientId == userId && estadosActivos.Contains(o.EstatOfertaId))
                    .Include(o => o.PortOrigen)
                    .Include(o => o.PortDesti)
                    .Include(o => o.AeroportOrigen)
                    .Include(o => o.AeroportDesti)
                    .Include(o => o.EstatOferta)
                    .OrderByDescending(o => o.DataCreacio)
                    .Select(o => new EnvioActivoDto
                    {
                        Id = o.Id,
                        Cliente = "Mi Envío",
                        RutaOrigen = o.TipusTransportId == 1 // 1 = Marítimo
                            ? (o.PortOrigen != null ? o.PortOrigen.Nom : "Puerto no asignado")
                            : (o.AeroportOrigen != null ? o.AeroportOrigen.Nom : "Aeropuerto no asignado"),

                        RutaDestino = o.TipusTransportId == 1
                            ? (o.PortDesti != null ? o.PortDesti.Nom : "Puerto no asignado")
                            : (o.AeroportDesti != null ? o.AeroportDesti.Nom : "Aeropuerto no asignado"),

                        Concepto = o.Concepto ?? "Carga General",
                        Estado = o.EstatOferta != null ? o.EstatOferta.Estat : "Estado desconocido",
                        EstadoId = o.EstatOfertaId,
                        FechaCreacion = o.DataCreacio.ToString("dd MMM, yyyy"),
                        TransportTypeId = o.TipusTransportId,
                        Precio = o.Valor ?? "Consultar"
                    })
                    .ToListAsync();

                return Ok(envios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener envíos: {ex.Message}" });
            }
        }
    

    [HttpGet("client/envios/detalle/{id}/{userId}")]
        public async Task<ActionResult<DetalleEnvioDto>> GetDetalleEnvioCliente(int id, int userId)
        {
            try
            {
                var oferta = await _context.Ofertes
                    .Include(o => o.EstatOferta)
                    .Include(o => o.PortOrigen).Include(o => o.PortDesti)
                    .Include(o => o.AeroportOrigen).Include(o => o.AeroportDesti)
                    // Incluimos el seguimiento y los pasos del tracking definidos en tu DB
                    .Include(o => o.OfertaSeguimientos)
                        .ThenInclude(s => s.TrackingStep)
                    // Filtramos por ID de oferta Y por ID de cliente para mayor seguridad
                    .FirstOrDefaultAsync(o => o.Id == id && o.ClientId == userId);

                if (oferta == null)
                    return NotFound(new { error = "Envío no encontrado o no pertenece al usuario" });

                var detalle = new DetalleEnvioDto
                {
                    Id = oferta.Id,
                    Cliente = "Mi Envío", // Podrías sacar el nombre real con otro include si fuera necesario
                    Concepto = oferta.Concepto ?? "Envío de mercancía",
                    EstadoActual = oferta.EstatOferta.Estat,
                    FechaCreacion = oferta.DataCreacio.ToString("dd/MM/yyyy"),

                    // Construcción de la ruta dinámica
                    RutaCompleta = oferta.TipusTransportId == 1 // Marítimo
                        ? $"{oferta.PortOrigen?.Nom ?? "N/A"} - {oferta.PortDesti?.Nom ?? "N/A"}"
                        : (oferta.TipusTransportId == 2 // Aéreo
                            ? $"{oferta.AeroportOrigen?.Nom ?? "N/A"} - {oferta.AeroportDesti?.Nom ?? "N/A"}"
                            : "Ruta Terrestre"),

                    // Mapeo de los pasos de seguimiento
                    TrackingSteps = oferta.OfertaSeguimientos
                        .OrderBy(s => s.Orden) // Orden lógico definido en tu tabla OfertaSeguimiento
                        .Select(s => new TrackingStepDTO
                        {
                            Id = s.Id,
                            // Nombre del paso desde la tabla maestra TrackingSteps
                            Titol = s.TrackingStep != null ? s.TrackingStep.Nom : "Estado",

                            // Fecha de cuando se completó el paso
                            DataHora = s.FechaCompletado.HasValue
                                ? s.FechaCompletado.Value.ToString("dd/MM/yyyy, HH:mm")
                                : "Pendiente",

                            TeDocument = !string.IsNullOrEmpty(s.DocumentoPath),
                            NomFitxer = s.DocumentoPath,
                            Comentari = s.Observaciones ?? "",
                            EstaCompletado = s.EstaCompletado ?? 0
                        }).ToList()
                };

                return Ok(detalle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el detalle del envío: " + ex.Message });
            }
        }
    }
}
