using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Data;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.DTOs.Productos;
using WebApi_ComprasStock.DTOs;
using WebApi_ComprasStock.Utilidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.IO;
using WebApi_ComprasStock.Servicios;

namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor= "productosCompras";

        public ProductosController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDBContext context,
            ILogger seriLogger, IMapper mapper,
            RoleManager<IdentityRole> roleManager,IAlmacenadorArchivos almacenadorArchivos)
            : base(context, seriLogger, mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.seriLogger = seriLogger;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        //____________________________________________________________________________________________________
        // GET: api/<ProductosController>
        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var respuesta = await Get<Producto, ProductoDTO>(paginacionDTO);
                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado generico Producto", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        // GET api/<ProductosController>/5
        [HttpGet("{id:int}", Name = "obtenerProducto")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            try
            {
                return await Get<Producto, ProductoDTO>(id);
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado generico Producto", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        [HttpGet("/productoCompleto/{id:int}", Name = "ProductoCompleto")]
        public async Task<ActionResult<ProductoDTOCompleto>> ProductoCompleto(int id)
        {
            try
            {
                var entidad = await context.Productos.Where(x => x.Id == id)
                    .Include(x => x.TipoProducto)
                    .Include(x => x.ProveedorProducto).ThenInclude(x => x.DatosProveedores)
                    .FirstOrDefaultAsync();

                if(entidad != null)
                {
                    ProductoDTOCompleto respuesta = new ProductoDTOCompleto();
                    respuesta = mapper.Map<ProductoDTOCompleto>(entidad);

                    return respuesta;
                }
                else
                {
                    seriLogger.Warning($"No se encontro ningún dato para el Producto {id}");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al solicitar datos Producto completo x id: {id}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        //POST api/<ProductosController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ProductoCreacionDTO creacionDTO)
        {
            try
            {
                var producto = mapper.Map<Producto>(creacionDTO);
                if (creacionDTO.ImagenGuardar != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await creacionDTO.ImagenGuardar.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(creacionDTO.ImagenGuardar.FileName);

                        string rutaImagen = string.Empty;
                        rutaImagen = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                            creacionDTO.ImagenGuardar.ContentType);
                        producto.Imagen = rutaImagen;

                    }
                }
                context.Add(producto);
                await context.SaveChangesAsync();

                var productoDTO = mapper.Map<ProductoDTO>(producto);

                return new CreatedAtRouteResult("obtenerProducto", new { id = producto.Id }, productoDTO);
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al generar datos de un nuevo Producto ", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        // PUT api/<ProductosController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ProductoCreacionDTO creacionDTO)
        {
            try
            {
                var productoDB = await context.Productos.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (productoDB == null)
                {
                    seriLogger.Warning($"No existe 1 Producto con id = {id}");
                    return NotFound($"No existe 1 Producto con id = {id}");
                }

                if (creacionDTO.ImagenGuardar != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await creacionDTO.ImagenGuardar.CopyToAsync(memoryStream);
                        var contenido = memoryStream.ToArray();
                        var extension = Path.GetExtension(creacionDTO.ImagenGuardar.FileName);

                        string rutaImagen = string.Empty;
                        rutaImagen = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                            creacionDTO.ImagenGuardar.ContentType,creacionDTO.Imagen);
                        productoDB.Imagen = rutaImagen;

                    }
                }
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al actualizar (PUT) datos del Producto con id = {id}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProductoPatchDTO> patchDocument)
        {
            return await Patch<Producto, ProductoPatchDTO>(id, patchDocument);
        }
        //____________________________________________________________________________________________________
        // DELETE api/<ProductosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Producto>(id);
        }
        //____________________________________________________________________________________________________
    }
}
