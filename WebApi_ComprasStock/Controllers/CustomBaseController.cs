using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApi_ComprasStock.Data;
using WebApi_ComprasStock.DTOs;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.Utilidades;
using System.Linq.Dynamic.Core;

namespace WebApi_ComprasStock.Controllers
{
    public class CustomBaseController:ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly ILogger seriLogger;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDBContext context,
            ILogger seriLogger, IMapper mapper)
        {
            this.context = context;
            this.seriLogger = seriLogger;
            this.mapper = mapper;
        }
        //_______________________________________________________________________________________________
        private string ObtenerNombreTipoT<TEntidad>() where TEntidad : class
        {
            Type TypeT = typeof(TEntidad);
            string nombreT = TypeT.Name;

            return nombreT;
        }
        //_______________________________________________________________________________________________
        private bool VerificarCampoEntidad<TEntidad>(string nombreCampo) where TEntidad : class
        {
            bool resultado;
            PropertyInfo[] listaPropiedades = typeof(TEntidad).GetProperties();
            var propiedad = listaPropiedades.Where(x => x.Name.Equals(nombreCampo)).FirstOrDefault();
            resultado = (propiedad != null) ? true : false;

            if(propiedad== null)
            {
                string nombreT = ObtenerNombreTipoT<TEntidad>();
                seriLogger.Error($"El listado de {nombreT} no contiene una propiedad {nombreCampo} ");
            }

            return resultado;
        }
        //_______________________________________________________________________________________________
        protected async Task<ActionResult<List<TDTO>>> Get<TEntidad,TDTO>(PaginacionDTO paginacionDTO) where TEntidad:class
        {
            // Get the Type object corresponding to MyClass.
            //Type TypeT = typeof(TDTO);
            //string nombreT = TypeT.Name;
            string nombreT = ObtenerNombreTipoT<TEntidad>();
            string parametroOrden = string.Empty;
            string tipoOrden = string.Empty;
            string parametroThenBy = string.Empty;
            string tipoOrdenThenBy = string.Empty;
            try
            {
                bool controlOrden = false;
                if (!string.IsNullOrEmpty(paginacionDTO.NombreCampoOrden))
                {
                    if (VerificarCampoEntidad<TEntidad>(paginacionDTO.NombreCampoOrden))
                    {
                        parametroOrden = paginacionDTO.NombreCampoOrden;
                        tipoOrden = paginacionDTO.OrdenAscendente == true ? "ascending" : "descending";
                        controlOrden = true;
                    }
                }
                var queryable = context.Set<TEntidad>().AsNoTracking().AsQueryable();
                await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
                if (controlOrden)
                {
                    queryable = queryable.OrderBy($"{ parametroOrden} {tipoOrden}");
                    if (!string.IsNullOrEmpty(paginacionDTO.CampoThenBy))
                    {
                        if (VerificarCampoEntidad<TEntidad>(paginacionDTO.CampoThenBy))
                        {
                            parametroThenBy = paginacionDTO.CampoThenBy;
                            tipoOrdenThenBy = paginacionDTO.OrdenThenBy == true ? "ascending" : "descending";
                            queryable = queryable.OrderBy($"{ parametroThenBy} {tipoOrdenThenBy}");
                        }
                    }
                }

                var entidades = await queryable.ToListAsync();
                if (entidades?.Count > 0)
                {
                    var dtos = mapper.Map<List<TDTO>>(entidades);

                    List<TDTO> lista = dtos.ToList();
                    var listaPaginada = FuncionesPaginar.PaginarListas(lista, paginacionDTO);

                    return listaPaginada;
                }
                else
                {
                    seriLogger.Warning($"El listado de {nombreT} no devuelve registros");
                    ModelState.AddModelError("NotFound", "No se encontraron registros para el listado solicitado");
                    return NotFound("No se encontraron registros para el listado solicitado");
                }
            }
            catch(Exception ex)
            {
                seriLogger.Error($"Error Generando Listado de {nombreT}",ex.Message);
                return BadRequest();
            }
        }
        //_____________________________________________________________________________________________
        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IImplementsId
        {
            var entidad = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if(entidad== null)
            {
                ModelState.AddModelError("NotFound", "No se encontro registro para la entidad solicitada");
                return NotFound();
            }

            return mapper.Map<TDTO>(entidad);
        }
        //_______________________________________________________________________________________________
        protected async Task<ActionResult> Post<TCreacion,TEntidad, TLectura>(TCreacion creacionDto, string nombreRuta)
            where TEntidad : class, IImplementsId
        {
            var entidad = mapper.Map<TEntidad>(creacionDto);
            context.Add(entidad);

            await context.SaveChangesAsync();

            var dtoLectura = mapper.Map<TLectura>(entidad);

            return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, dtoLectura);
        }
        //_______________________________________________________________________________________________
        protected async Task<ActionResult> Put<TModificador, TEntidad>(int id, TModificador modificadorDto)
            where TEntidad : class, IImplementsId
        {
            var entidad = mapper.Map<TEntidad>(modificadorDto);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return NoContent();
        }
        //_______________________________________________________________________________________________
        protected async Task<ActionResult> Patch<TEntidad,TDTO>(int id, JsonPatchDocument<TDTO> patchDocument) 
             where TDTO : class
            where TEntidad : class, IImplementsId
        {
            string nombreTDto = ObtenerNombreTipoT<TDTO>();
            if (patchDocument == null)
            {
                seriLogger.Error($"Error leyendo Json DTO de {nombreTDto}");
                return BadRequest();
            }

            var entidadDB = await context.Set<TEntidad>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if(entidadDB == null)
            {
                seriLogger.Error($"No se encontro 1 registro para {nombreTDto} con Id= {id}");
                return NotFound();
            }

            var entidadDTO = mapper.Map<TDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO,ModelState);
            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                seriLogger.Error($"Se produjo un error al validar el modelo" +
                    $" en metodo Patch con la entidad {nombreTDto} con Id= {id}");
                return BadRequest();
            }

            mapper.Map(entidadDTO, entidadDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        //_______________________________________________________________________________________________
        protected async Task<ActionResult> Delete< TEntidad>(int id) where TEntidad : class, IImplementsId, new ()
        {
            var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new TEntidad() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
