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
using WebApi_ComprasStock.DTOs.ProveedoresDTOs;
using WebApi_ComprasStock.DTOs;
using WebApi_ComprasStock.Utilidades;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApi_ComprasStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public ProveedoresController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDBContext context,
            ILogger seriLogger, IMapper mapper,
            RoleManager<IdentityRole> roleManager)
            :base(context,seriLogger,mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.seriLogger = seriLogger;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        //____________________________________________________________________________________________________
        // GET: api/<ProveedoresController>
        [HttpGet]
        public async Task<ActionResult<List<ProveedorDTO>>> Get([FromQuery]PaginacionDTO paginacionDTO)
        {
            try
            {
                var respuesta = await Get<DatosProveedores, ProveedorDTO>(paginacionDTO);

                return respuesta;
            }
            catch(Exception ex)
            {
                seriLogger.Error("Error al solicitar listado generico Proveedores", ex.Message);
                return BadRequest();
            }
        }
        //____________________________________________________________________________________________________
        // GET api/<ProveedoresController>/5
        [HttpGet("{id:int}", Name = "obtenerProveedor")]
        public async Task<ActionResult<ProveedorDTO>> Get(int id)
        {
            try
            {
                return await Get<DatosProveedores, ProveedorDTO>(id);
            }
            catch (Exception ex)
            {
                seriLogger.Error("Error al solicitar listado generico Proveedores", ex.Message);
                return BadRequest();
            }
        }

        //____________________________________________________________________________________________________
        //POST api/<ProveedoresController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProveedorCreacionDTO creacionDTO)
        {
            return await Post<ProveedorCreacionDTO, DatosProveedores, ProveedorDTO>(creacionDTO, "obtenerProveedor");
        }

        //____________________________________________________________________________________________________
        // PUT api/<ProveedoresController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProveedorCreacionDTO creacionDTO)
        {
            return await Put<ProveedorCreacionDTO, DatosProveedores>(id, creacionDTO);
        }
        //____________________________________________________________________________________________________
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProveedorPatchDTO> patchDocument)
        {
            return await Patch<DatosProveedores, ProveedorPatchDTO>(id, patchDocument);
        }
        //____________________________________________________________________________________________________
        // DELETE api/<ProveedoresController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<DatosProveedores>(id);
        }
        //____________________________________________________________________________________________________
    }
}
