using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Utilidades;
using WebApi_ComprasStock.Validaciones;

namespace WebApi_ComprasStock.DTOs.ProveedoresDTOs
{
    public class ProveedorCreacionDTO: ProveedorPatchDTO
    {

        [ModelBinder(BinderType = typeof(CustomTypeBinder<List<int>>))]
        public List<int> ProductosIds { get; set; }

    }
}
