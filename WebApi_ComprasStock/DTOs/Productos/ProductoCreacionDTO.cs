using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.Validaciones;
using WebApi_ComprasStock.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class ProductoCreacionDTO: ProductoPatchDTO
    {

        public string Imagen { get; set; }

        public List<int> ProveedorIds { get; set; }

        [TamañoArchivoValidacion(pesoMaximoEnMB:4)]
        [TipoArchivoValidacion( grupoTipoArchivo:GrupoTipoArchivo.Imagen)]
        public IFormFile ImagenGuardar { get; set; }

        [ModelBinder(BinderType = typeof(CustomTypeBinder<List<int>>))]
        public List<int> ProveedoresIds { get; set; }

    }
}
