using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi_ComprasStock.Data;
using WebApi_ComprasStock.DTOs;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.Utilidades;

namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public CuentasController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDBContext context,
            ILogger seriLogger, IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.seriLogger = seriLogger;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        //_________________________________________________________________-_________________________________________________
        //_________________________________________________________________-_________________________________________________
        [HttpPost("crear")]
        public async Task<ActionResult<RespuestaAutenticacion>> Crear([FromBody] UsuarioCreacionDTO usuarioCreacion)
        {
            var usuario = new ApplicationUser
            {
                UserName = usuarioCreacion.Email,
                Email = usuarioCreacion.Email
            };

            var resultado = await userManager.CreateAsync(usuario, usuarioCreacion.Password);

            if (resultado.Succeeded)
            {
                CredencialesUsuario credenciales = new CredencialesUsuario()
                {
                    Password = usuarioCreacion.Password,
                    Email = usuarioCreacion.Email
                };
                return await ConstruirToken(credenciales, usuarioCreacion.Nombres, usuarioCreacion.Apellidos);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }
        //_________________________________________________________________-_________________________________________________
        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credenciales, string nombres, string apellidos)
        {
            //... Generar y/o Recuperar Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, credenciales.Email),
                new Claim(ClaimTypes.Name, credenciales.Email),
                new Claim("Nombres", nombres),
                new Claim("Apellidos", apellidos),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var usuario = await userManager.FindByEmailAsync(credenciales.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);
            //........................................................................
            //......... Generar JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };
        }
        //_________________________________________________________________-_________________________________________________
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login([FromBody] CredencialesUsuario credenciales)
        {
            var resultado = await signInManager.PasswordSignInAsync(credenciales.Email, credenciales.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                var usuario = await userManager.FindByEmailAsync(credenciales.Email);
                return await ConstruirToken(credenciales, usuario.Nombres, usuario.Apellidos);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }
        //_________________________________________________________________-_________________________________________________
        [HttpPost("AsignarAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> AsignarAdmin([FromBody] string usuarioId)
        {
            var usuario = await userManager.FindByIdAsync(usuarioId);
            await userManager.AddClaimAsync(usuario, new Claim("role", "admin"));

            return NoContent();
        }
        //_________________________________________________________________-_________________________________________________
        [HttpPost("RemoverAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin([FromBody] string usuarioId)
        {
            var usuario = await userManager.FindByIdAsync(usuarioId);
            await userManager.RemoveClaimAsync(usuario, new Claim("role", "admin"));

            return NoContent();
        }
        //_________________________________________________________________-_________________________________________________
        [HttpGet("listadoUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<UsuarioDTO>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var usuarios = queryable.OrderBy(o => o.Email).ToList();
            var usuariosPaginados = FuncionesPaginar.PaginarListas(usuarios, paginacionDTO).ToList();

            return mapper.Map<List<UsuarioDTO>>(usuariosPaginados);
        }
        //_________________________________________________________________-_________________________________________________
        [HttpPut("EditarUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> EditarUsuario(UsuarioEditar usuarioEditar)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var userId = user.Id;
                    var usuarioBD = await context.ApplicationUser.Where(x => x.Id == userId).FirstOrDefaultAsync();
                    usuarioBD.Nombres = usuarioEditar.Nombres;
                    usuarioBD.Apellidos = usuarioEditar.Apellidos;
                    if (!string.IsNullOrWhiteSpace(usuarioEditar.Telefono))
                    {
                        usuarioBD.PhoneNumber = usuarioEditar.Telefono;
                        usuarioBD.PhoneNumberConfirmed = true;
                    }

                    await context.SaveChangesAsync();

                    return NoContent();
                }
                seriLogger.Error("Error al intentar actualizar los datos");
                return BadRequest();
            }
            catch (Exception)
            {
                seriLogger.Error("Error al intentar actualizar los datos");
                return BadRequest();
            }

        }
        //_________________________________________________________________-_________________________________________________
        [HttpPost("CrearRoles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> CrearRoles([FromBody] string roleName)
        {
            try
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    return NotFound($"Ya existe un rol con el nombre: {roleName}");
                }

                IdentityRole newRole = new IdentityRole()
                {
                    Name = roleName
                };
                IdentityResult result = await roleManager.CreateAsync(newRole);
                if (result.Succeeded) { return NoContent(); }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Error al intentar crear un role con nombre {roleName}");
                    sb.AppendLine("Detalle: ");
                    foreach (IdentityError errors in result.Errors)
                    {
                        sb.AppendLine(errors.Description);
                    }
                    seriLogger.Error(sb.ToString());
                    return BadRequest($"No se puede crear el rol: {roleName}");
                }
            }
            catch (Exception)
            {
                seriLogger.Error($"Error al intentar crear un role con nombre {roleName}");
                return BadRequest();
            }
        }
        //_________________________________________________________________-_________________________________________________
        [HttpGet("listadoRoles")]
        public async Task<ActionResult<List<RolesDTO>>> ListadoRoles([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = roleManager.Roles.AsQueryable();
            if (queryable == null)
            {
                seriLogger.Error("Listado de Roles devuelve null");
                return NotFound();
            }
            else
            {
                double cantidad = await queryable.CountAsync();
                if (cantidad == 0)
                {
                    seriLogger.Error("Listado de Roles no devuelve registros");
                    return NotFound();
                }
            }
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var roles = queryable.OrderBy(o => o.NormalizedName).ToList();
            var rolesPaginados = FuncionesPaginar.PaginarListas(roles, paginacionDTO).ToList();

            return mapper.Map<List<RolesDTO>>(rolesPaginados);
        }
        //_________________________________________________________________-_________________________________________________
    }
}
