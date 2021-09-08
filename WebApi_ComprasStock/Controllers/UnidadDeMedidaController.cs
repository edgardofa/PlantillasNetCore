using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_ComprasStock.Data;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.DTOs.Diccionarios;
using WebApi_ComprasStock.DTOs;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadDeMedidaController : CustomBaseController
    {
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;

        public UnidadDeMedidaController(ApplicationDBContext context,
            ILogger seriLogger, IMapper mapper)
            : base(context, seriLogger, mapper)
        {
            this.context = context;
            this.seriLogger = seriLogger;
            this.mapper = mapper;
        }
        //______________________________________________________________________________________________________________
        // GET api/<UnidadDeMedidaController>/5
        [HttpGet("{id:int}", Name = "obtenerUnidadDeMedida")]
        public async Task<ActionResult<UnidadDeMedidaDTO>> Get(int id)
        {
            try
            {
                return await Get<UnidadDeMedida, UnidadDeMedidaDTO>(id);
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar datos Unidad de Medida x id", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpGet("listadoUnidadDeMedida")]
        public async Task<ActionResult<List<UnidadDeMedidaDTO>>> listadoUnidadDeMedida([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                var respuesta = await Get<UnidadDeMedida, UnidadDeMedidaDTO>(paginacionDTO);

                return respuesta;
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado de  Unidad de Medida", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        [HttpGet("/umActivas/{activa}", Name = "UMedidaActivas")]
        public async Task<ActionResult<List<UnidadDeMedidaDTO>>> UMedidaActivas(bool activa)
        {
            try
            {
                var entidades = await context.UnidadDeMedida.Where(x => x.Activo == activa).ToListAsync();
                if(entidades?.Count > 0)
                {
                    List<UnidadDeMedidaDTO> respuesta = new List<UnidadDeMedidaDTO>();
                    respuesta = mapper.Map<List<UnidadDeMedidaDTO>>(entidades);

                    return respuesta;
                }
                else
                {
                    string aux = (activa) ? "Activas" : "Inactivas";
                    seriLogger.Warning($"El listado de Unidades de Medida {aux} no devuelve registros");
                    return NotFound($"El listado de Unidades de Medida {aux} no devuelve registros");
                }
            }
            catch (Exception ex)
            {
                string aux = (activa) ? "Activas" : "Inactivas";
                seriLogger.Error($"Error al solicitar listado de Unidades de Medida {aux}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        //POST api/<UnidadDeMedidaController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UnidadDeMedidaCreacionDTO creacionDTO)
        {
            try
            {
                return await Post<UnidadDeMedidaCreacionDTO, UnidadDeMedida, UnidadDeMedidaDTO>
                    (creacionDTO, "obtenerUnidadDeMedida");
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al generar la  Unidad de Medida ", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UnidadDeMedidaCreacionDTO creacionDTO)
        {
            try
            {
                return await Put<UnidadDeMedidaCreacionDTO, UnidadDeMedida>(id, creacionDTO);
            }
            catch (Exception ex)
            {
                seriLogger.Error($"Error al modificar la  Unidad de Medida con Id: {id}", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
    }
}
