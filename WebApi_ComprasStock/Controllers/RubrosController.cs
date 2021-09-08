using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Data;
using WebApi_ComprasStock.DTOs;
using WebApi_ComprasStock.DTOs.Productos;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.Utilidades;

namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RubrosController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;

        public RubrosController(UserManager<ApplicationUser> userManager,
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
        // GET api/<RubrosController>/5
        [HttpGet("{id:int}", Name = "obtenerRubro")]
        public async Task<ActionResult<RubroDTO>> Get(int id)
        {
            try
            {
                return await Get<Rubros, RubroDTO>(id);
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar datos Rubro x id", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        // GET api/<RubrosController>/5
        /// <summary>
        /// Recupera el Rubro solicitado e incluye los datos de las categorías asociadas
        /// </summary>
        /// <param name="id">Id del Rubro a consultar</param>
        /// <returns></returns>
        [HttpGet("/rubroCompleta/{id:int}", Name = "RubroCompleta")]
        public async Task<ActionResult<RubroDTOCompleta>> RubroCompleta(int id)
        {
            try
            {
                var entidad = await context.Rubros.Where(x => x.Id == id)
                    .Include(categoria => categoria.Categorias)
                    .FirstOrDefaultAsync();
                var respuesta = mapper.Map<RubroDTOCompleta>(entidad);
                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar datos Rubro completa x id", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        [HttpGet("listadoRubros")]
        public async Task<ActionResult<List<RubroDTO>>> ListadoRubros([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var respuesta = await Get<Rubros, RubroDTO>(paginacionDTO);

                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado de Rubros", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        //POST api/<RubrosController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RubroCreacionDTO creacionDTO)
        {
            try
            {
                return await Post<RubroCreacionDTO, Rubros, RubroDTO>(creacionDTO, "obtenerRubro");
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al generar el nuevo Rubro", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RubroCreacionDTO creacionDTO)
        {
            try
            {
                return await Put<RubroCreacionDTO, Rubros>(id, creacionDTO);
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al modificar el Rubro con Id: {id}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________

    }
}
