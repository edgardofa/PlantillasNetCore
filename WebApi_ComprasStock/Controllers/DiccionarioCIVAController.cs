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
using WebApi_ComprasStock.DTOs.Diccionarios;
using WebApi_ComprasStock.DTOs;
using Microsoft.EntityFrameworkCore;


namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiccionarioCIVAController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;

        public DiccionarioCIVAController(UserManager<ApplicationUser> userManager,
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
        // GET api/<DiccionarioCIVAController>/5
        [HttpGet("{id:int}", Name = "obtenerCondIVA")]
        public async Task<ActionResult<Diccionario_CIVA_DTO>> Get(int id)
        {
            try
            {
                return await Get<Diccionario_CIVA, Diccionario_CIVA_DTO>(id);
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar datos Cond. IVA x id", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpGet("listadoCondIVA")]
        public async Task<ActionResult<List<Diccionario_CIVA_DTO>>> listadoCondIVA([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var respuesta = await Get<Diccionario_CIVA, Diccionario_CIVA_DTO>(paginacionDTO);

                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado de Cond. IVA", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        [HttpGet("/CondIVAActivas/{activa}", Name = "CondIVAActivas")]
        public async Task<ActionResult<List<Diccionario_CIVA_DTO>>> CondIVAActivas(bool activa)
        {
            try
            {
                var entidades = await context.Diccionario_CIVA.Where(x => x.Activo == activa).ToListAsync();
                if (entidades?.Count > 0)
                {
                    List<Diccionario_CIVA_DTO> respuesta = new List<Diccionario_CIVA_DTO>();
                    respuesta = mapper.Map<List<Diccionario_CIVA_DTO>>(entidades);

                    return respuesta;
                }
                else
                {
                    string aux = (activa) ? "Activas" : "Inactivas";
                    seriLogger.Warning($"El listado de Cond. IVA {aux} no devuelve registros");
                    return NotFound($"El listado de Cond. IVA {aux} no devuelve registros");
                }
            }
            catch (Exception ex)
            {
                string aux = (activa) ? "Activas" : "Inactivas";
                seriLogger.Error($"Error al solicitar listado de Cond. IVA {aux}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        //POST api/<DiccionarioCIVAController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Diccionario_CIVA_CreacionDTO creacionDTO)
        {
            try
            {
                return await Post<Diccionario_CIVA_CreacionDTO, Diccionario_CIVA, Diccionario_CIVA_DTO>
                    (creacionDTO, "obtenerCondIVA");
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al generar la Cond. de IVA ", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Diccionario_CIVA_CreacionDTO creacionDTO)
        {
            try
            {
                return await Put<Diccionario_CIVA_CreacionDTO, Diccionario_CIVA>(id, creacionDTO);
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al modificar la Cond. de IVA con Id: {id}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
    }
}
