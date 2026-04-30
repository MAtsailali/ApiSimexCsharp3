using System.Net.Mime;
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

        [HttpGet("client/ofertes/accepted/{clientId}")]
        public async Task<ActionResult> ObtenerEnviosAceptados(int clientId)
        {
            try
            {
                var enviosAceptados = await _context.Ofertes
                    .Where(o => o.ClientId == clientId && o.EstatOfertaId == 1)
                    .Include(o => o.PortOrigen)
                    .Include(o => o.PortDesti)
                    .Include(o => o.AeroportOrigen)
                    .Include(o => o.AeroportDesti)
                    .OrderByDescending(o => o.DataCreacio)
                    .Select(o => new
                    {
                        id = o.Id,
                        price = string.IsNullOrEmpty(o.Valor) ? "Consultar" : o.Valor,
                        route = o.PortOrigen != null && o.PortDesti != null
                            ? $"{o.PortOrigen.Nom} / {o.PortDesti.Nom}"
                            : o.AeroportOrigen != null && o.AeroportDesti != null
                                ? $"{o.AeroportOrigen.Nom} / {o.AeroportDesti.Nom}"
                                : "Ruta pendiente",
                        description = o.Concepto ?? "Envio de mercancia",
                        rejection_reason = "Operacion activa",
                        transport_type_id = o.TipusTransportId
                    })
                    .ToListAsync();

                return Ok(enviosAceptados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("client/presupuestos/{clientId}")]
        public async Task<ActionResult> ObtenerPresupuestos(int clientId)
        {
            try
            {
                var presupuestos = await _context.Ofertes
                    .Where(o => o.ClientId == clientId)
                    .Include(o => o.PortOrigen)
                    .Include(o => o.PortDesti)
                    .Include(o => o.AeroportOrigen)
                    .Include(o => o.AeroportDesti)
                    .Include(o => o.EstatOferta)
                    .OrderByDescending(o => o.DataCreacio)
                    .Select(o => new
                    {
                        id = o.Id,
                        price = string.IsNullOrEmpty(o.Valor) ? "Consultar" : o.Valor,
                        route = o.PortOrigen != null && o.PortDesti != null
                            ? $"{o.PortOrigen.Nom} / {o.PortDesti.Nom}"
                            : o.AeroportOrigen != null && o.AeroportDesti != null
                                ? $"{o.AeroportOrigen.Nom} / {o.AeroportDesti.Nom}"
                                : "Ruta pendiente",
                        description = o.Concepto ?? "Envio de mercancia",
                        rejection_reason = o.RaoRebuig,
                        transport_type_id = o.TipusTransportId,
                        status = o.EstatOferta.Estat,
                        status_id = o.EstatOfertaId,
                        date = o.DataCreacio
                    })
                    .ToListAsync();

                return Ok(presupuestos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("client/presupuestos/{presupuestoId}/accept")]
        public async Task<IActionResult> AceptarPresupuesto(int presupuestoId, [FromBody] DecisionPresupuestoRequest request)
        {
            try
            {
                var presupuesto = await _context.Ofertes
                    .FirstOrDefaultAsync(o => o.Id == presupuestoId && o.ClientId == request.ClientId);

                if (presupuesto == null)
                {
                    return NotFound(new { error = "Presupuesto no encontrado" });
                }

                presupuesto.EstatOfertaId = 1;
                presupuesto.RaoRebuig = null;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Presupuesto aceptado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("client/presupuestos/{presupuestoId}/reject")]
        public async Task<IActionResult> RechazarPresupuesto(int presupuestoId, [FromBody] DecisionPresupuestoRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RejectionReason))
                {
                    return BadRequest(new { error = "El motivo de rechazo es obligatorio" });
                }

                var presupuesto = await _context.Ofertes
                    .FirstOrDefaultAsync(o => o.Id == presupuestoId && o.ClientId == request.ClientId);

                if (presupuesto == null)
                {
                    return NotFound(new { error = "Presupuesto no encontrado" });
                }

                presupuesto.EstatOfertaId = 4;
                presupuesto.RaoRebuig = request.RejectionReason.Trim();
                await _context.SaveChangesAsync();

                return Ok(new { message = "Presupuesto rechazado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("client/envios/{ofertaId}/tracking")]
        public async Task<IActionResult> ObtenerTrackingEnvio(int ofertaId)
        {
            try
            {
                var oferta = await _context.Ofertes
                    .Include(o => o.PortOrigen)
                    .Include(o => o.PortDesti)
                    .Include(o => o.AeroportOrigen)
                    .Include(o => o.AeroportDesti)
                    .Include(o => o.EstatOferta)
                    .Include(o => o.OfertaSeguimientos)
                        .ThenInclude(s => s.TrackingStep)
                    .FirstOrDefaultAsync(o => o.Id == ofertaId);

                if (oferta == null)
                {
                    return NotFound(new { error = "Envio no encontrado" });
                }

                var origen = ObtenerOrigen(oferta);
                var destino = ObtenerDestino(oferta);
                var hitos = oferta.OfertaSeguimientos
                    .OrderBy(s => s.Orden)
                    .Select(s => new
                    {
                        titulo = s.TrackingStep.Nom ?? "Seguimiento",
                        descripcion = string.IsNullOrWhiteSpace(s.Observaciones)
                            ? (s.EstaCompletado == 1 ? "Completado" : "Pendiente")
                            : s.Observaciones,
                        fecha = s.FechaCompletado?.ToString("yyyy-MM-dd HH:mm"),
                        completado = s.EstaCompletado == 1
                    })
                    .ToList();

                var completados = hitos.Count(h => h.completado);
                var progreso = hitos.Count > 0
                    ? (int)Math.Round(completados * 100.0 / hitos.Count)
                    : CalcularProgresoEnvio(oferta.EstatOfertaId);
                var ultimoHito = hitos.LastOrDefault(h => h.completado);
                var estado = ultimoHito?.titulo ?? ObtenerEstadoEnvio(oferta);

                return Ok(new
                {
                    estado,
                    progreso,
                    origen,
                    destino,
                    ruta = $"{origen} -> {destino}",
                    llegada_estimada = (oferta.DataValidessaFina ?? oferta.DataCreacio.AddDays(15)).ToString("yyyy-MM-dd"),
                    hitos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("client/ofertes/{ofertaId}/documents/{nombreArchivo}")]
        public async Task<IActionResult> DescargarDocumento(int ofertaId, string nombreArchivo)
        {
            try
            {
                var nombreArchivoSeguro = Path.GetFileName(nombreArchivo);
                var rutaArchivo = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "uploads",
                    "ofertes",
                    ofertaId.ToString(),
                    nombreArchivoSeguro
                );

                if (!System.IO.File.Exists(rutaArchivo))
                {
                    return NotFound(new { error = "Documento no disponible" });
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(rutaArchivo);
                return File(bytes, ObtenerTipoContenido(nombreArchivoSeguro), nombreArchivoSeguro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private static string ObtenerOrigen(Oferte oferta)
        {
            return oferta.PortOrigen?.Nom
                ?? oferta.AeroportOrigen?.Nom
                ?? "Origen pendiente";
        }

        private static string ObtenerDestino(Oferte oferta)
        {
            return oferta.PortDesti?.Nom
                ?? oferta.AeroportDesti?.Nom
                ?? "Destino pendiente";
        }

        private static string ObtenerEstadoEnvio(Oferte oferta)
        {
            if (!string.IsNullOrWhiteSpace(oferta.EstatOferta?.Estat))
            {
                return oferta.EstatOferta.Estat;
            }

            return oferta.EstatOfertaId switch
            {
                1 => "En transito",
                4 => "Rechazado",
                _ => "Pendiente"
            };
        }

        private static int CalcularProgresoEnvio(int estadoOfertaId)
        {
            return estadoOfertaId switch
            {
                1 => 35,
                4 => 0,
                _ => 15
            };
        }

        private static string ObtenerTipoContenido(string nombreArchivo)
        {
            var extension = Path.GetExtension(nombreArchivo).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => MediaTypeNames.Application.Pdf,
                ".jpg" or ".jpeg" => MediaTypeNames.Image.Jpeg,
                ".png" => "image/png",
                _ => MediaTypeNames.Application.Octet
            };
        }
    }

    public class DecisionPresupuestoRequest
    {
        public int ClientId { get; set; }
        public string? RejectionReason { get; set; }
    }
}

