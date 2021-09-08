using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Validaciones;
using WebApi_ComprasStock.DTOs.Productos;
using WebApi_ComprasStock.DTOs.Diccionarios;

namespace WebApi_ComprasStock.DTOs.ProveedoresDTOs
{
    public class ProveedorDTO
    {
        public int Id { get; set; }

        public string RSocial { get; set; }

        public string Cuit { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public string Direccion { get; set; }

        public string Telefonos { get; set; }

        public int CIVA { get; set; }

        public bool Activo { get; set; }

        public Diccionario_CIVA_DTO diccionario_CIVA { get; set; }

    }
}
