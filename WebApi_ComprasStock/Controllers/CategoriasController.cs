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
using Microsoft.EntityFrameworkCore;

namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;

        public CategoriasController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDBContext context,
            ILogger seriLogger, IMapper mapper)
            : base(context, seriLogger, mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.seriLogger = seriLogger;
            this.mapper = mapper;
        }
        //______________________________________________________________________________________________________________
        // GET api/<CategoriasController>/5
        [HttpGet("{id:int}", Name = "obtenerCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                return await Get<Categorias, CategoriaDTO>(id);
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar datos Categoria x id", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        // GET api/<CategoriasController>/5
        [HttpGet("/categoriaCompleta/{id:int}", Name = "CategoriaCompleta")]
        public async Task<ActionResult<CategoriaDTOCompleta>> CategoriaCompleta(int id)
        {
            try
            {
                var entidad = await context.Categorias.Where(x => x.Id == id)
                    .Include(tipo => tipo.TipoProductos)
                    .Include(rubro => rubro.Rubro)
                    .FirstOrDefaultAsync();
                var respuesta = mapper.Map<CategoriaDTOCompleta>(entidad);
                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar datos Categoria completa x id", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpGet("listadoCategorias")]
        public async Task<ActionResult<List<CategoriaDTO>>> ListadoCategorias([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var respuesta = await Get<Categorias, CategoriaDTO>(paginacionDTO);

                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado de Categorias", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        //POST api/<CategoriasController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaCreacionDTO creacionDTO)
        {
            try
            {
                return await Post<CategoriaCreacionDTO, Categorias, CategoriaDTO>(creacionDTO, "obtenerCategoria");
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al generar la nueva Categoria", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaCreacionDTO creacionDTO)
        {
            try
            {
                return await Put<CategoriaCreacionDTO, Categorias>(id, creacionDTO);
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al modificar la Categoria con Id: {id}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________

    }
}
