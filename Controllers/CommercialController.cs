namespace ApiSimexCsharp.Controllers;
using ApiSimexCsharp.DTO;
using ApiSimexCsharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/")]
public class CommercialController : ControllerBase
{
    private readonly Simex01Context _context;

    public CommercialController(Simex01Context context)
    {
        _context = context;
    }


    [HttpGet("commercial/dashboard/{userId}")]
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


    [HttpGet("commercial/ofertes/sent/{userId}")]
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

 
    [HttpGet("commercial/ofertes/accepted/{userId}")]
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

  
    [HttpGet("commercial/ofertes/rejected/{userId}")]
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

    [HttpPost("commercial/register-client")]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientRequestDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Validar Moneda (Esta tabla suele ser fija, no creamos nuevas)
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.Id == dto.CurrencyId);

            if (currency == null) return BadRequest(new { error = "La moneda especificada no es válida." });

            // 2. Manejo de Industria (Buscar o Crear)
            var industria = await _context.Industria
                .FirstOrDefaultAsync(i => i.Categoria == dto.IndustryName);
            if (industria == null)
            {
                industria = new Industrium { Categoria = dto.IndustryName };
                _context.Industria.Add(industria);
                await _context.SaveChangesAsync();
            }

            // 3. Crear Company
            var newCompany = new Company
            {
                CompanyName = dto.CompanyName,
                IndustriaId = industria.Id,
            };
            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();

            // 4. Crear Usuario (RolId 3 fijo)
            var newUser = new Usuari
            {
                Correu = dto.Correu,
                Nom = dto.Nom,
                Cognoms = dto.Cognoms,
                Tlfn = dto.Tlfn,
                Contrasenya = BCrypt.Net.BCrypt.HashPassword("123456"),
                RolId = 3,
                CompanyId = newCompany.Id,
                Status = 1
            };
            _context.Usuaris.Add(newUser);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return Ok(new { message = "Registro completo" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { error = "Error interno: " + ex.Message });
        }
    }


    [HttpGet("commercial/industria")]
    public async Task<ActionResult<IEnumerable<Industrium>>> GetIndustrias()
    {
        return await _context.Industria
            .OrderBy(i => i.Categoria)
            .ToListAsync();
    }

    [HttpGet("commercial/currency")]
    public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
    {
        
        return await _context.Currencies.ToListAsync();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> PostLogin([FromBody] LoginRequest loginReq)
    {

        var user = await _context.Usuaris.FirstOrDefaultAsync(u => u.Correu == loginReq.Email);
        if (user == null) return NotFound("Usuario no encontrado");


        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("Esta_Es_Una_Clave_Muy_Larga_Y_Segura_De_Al_Menos_64_Caracteres_1234567890");


        var claims = new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Correu),
        new Claim(ClaimTypes.Role, user.RolId.ToString()),
         
        // Genera un ID único para este token específico.
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    });

        // 4. Configuramos el descriptor del token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        // 5. Creamos el token físico (string)
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(tokenConfig);

        // 6. Devolvemos la respuesta que espera tu Android
        return Ok(new LoginResponseDTO
        {
            usuari = new LoginDTO
            {
                    Id = user.Id,
                    correu = user.Correu,
                    contrasenya = user.Contrasenya,
                    nom = user.Nom,
                    cognoms = user.Cognoms,
                    rolId = user.RolId,
                    companyId = user.CompanyId,
                    status = user.Status,
                    tlfn = user.Tlfn,
                    ultima_conex = user.UltimaConex
        },
       token = tokenString
    });
    }
    public class LoginRequest { public string Email { get; set; } }
}