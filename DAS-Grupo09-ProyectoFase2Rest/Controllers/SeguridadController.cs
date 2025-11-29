using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAS_Grupo09_ProyectoFase2Rest.Data;
using DAS_Grupo09_ProyectoFase2Rest.DTOs;
using BCrypt.Net;

namespace DAS_Grupo09_ProyectoFase2Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguridadController : ControllerBase
    {
        private readonly EnvioPaqueteContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SeguridadController> _logger;

        public SeguridadController(
            EnvioPaqueteContext context,
            IConfiguration configuration,
            ILogger<SeguridadController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para autenticación de usuarios.
        /// </summary>
        /// <param name="request">Credenciales del usuario (usuario y clave).</param>
        /// <returns>Token JWT si las credenciales son válidas.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validar que el request no sea nulo ni venga vacío
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Usuario) ||
                    string.IsNullOrWhiteSpace(request.Clave))
                {
                    return Ok(new LoginResponse
                    {
                        Code = -1,
                        Msj = "Usuario y clave son requeridos"
                    });
                }

                // Buscar el usuario por username e incluir el rol
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Username == request.Usuario);

                // Validar si el usuario existe
                if (usuario == null)
                {
                    _logger.LogWarning("Intento de login fallido: Usuario {Usuario} no encontrado", request.Usuario);
                    return Ok(new LoginResponse
                    {
                        Code = -1,
                        Msj = "Usuario o clave incorrectos"
                    });
                }

                // Validar si el usuario está activo
                if (!usuario.EstaActivo)
                {
                    _logger.LogWarning("Intento de login con usuario inactivo: {Usuario}", request.Usuario);
                    return Ok(new LoginResponse
                    {
                        Code = -1,
                        Msj = "El usuario está inactivo. Contacte al administrador"
                    });
                }

                // =============================
                // VALIDAR CONTRASEÑA (HASH O PLANO)
                // =============================
                bool passwordValida = false;

                try
                {
                    // Si la clave en BD parece un hash BCrypt (comienza con $2)
                    if (!string.IsNullOrEmpty(usuario.Clave) &&
                        usuario.Clave.StartsWith("$2"))
                    {
                        // Validar con BCrypt
                        passwordValida = BCrypt.Net.BCrypt.Verify(request.Clave, usuario.Clave);
                    }
                    else
                    {
                        // Clave en texto plano (como tu usuario admin actual)
                        passwordValida = usuario.Clave == request.Clave;
                    }
                }
                catch (Exception exBcrypt)
                {
                    // Si BCrypt falla (por ejemplo "Invalid salt version"),
                    // hacemos un fallback a comparación directa.
                    _logger.LogWarning(exBcrypt,
                        "Error verificando contraseña con BCrypt para usuario {Usuario}. Se intentará comparación simple.",
                        request.Usuario);

                    passwordValida = usuario.Clave == request.Clave;
                }

                if (!passwordValida)
                {
                    _logger.LogWarning("Intento de login fallido: Contraseña incorrecta para usuario {Usuario}", request.Usuario);
                    return Ok(new LoginResponse
                    {
                        Code = -1,
                        Msj = "Usuario o clave incorrectos"
                    });
                }

                // Generar el token JWT
                var token = GenerarTokenJWT(usuario);

                _logger.LogInformation("Login exitoso para usuario: {Usuario}", usuario.Username);

                // Retornar respuesta exitosa
                return Ok(new LoginResponse
                {
                    Code = 0,
                    Msj = "Autenticación exitosa",
                    Token = token,
                    Usuario = new UsuarioInfo
                    {
                        Id = usuario.Id,
                        Username = usuario.Username,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Correo = usuario.Correo,
                        Rol = usuario.Rol?.Nombre ?? "Sin rol",
                        IdRol = usuario.IdRol
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de login");
                return Ok(new LoginResponse
                {
                    Code = -1,
                    Msj = $"Error en el servidor: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Endpoint temporal para generar hashes BCrypt (solo si quieres probar hashing).
        /// </summary>
        [HttpPost("generar-hash")]
        public ActionResult<object> GenerarHash([FromBody] string password)
        {
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(password);
                bool valido = BCrypt.Net.BCrypt.Verify(password, hash);

                return Ok(new
                {
                    password,
                    hash,
                    valido
                });
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Genera un token JWT para el usuario autenticado.
        /// </summary>
        private string GenerarTokenJWT(Models.Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey no configurada");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims del token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim("Username", usuario.Username),
                new Claim(ClaimTypes.GivenName, usuario.Nombre),
                new Claim(ClaimTypes.Surname, usuario.Apellido),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Sin rol"),
                new Claim("IdRol", usuario.IdRol.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Configurar expiración del token
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "480");
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
