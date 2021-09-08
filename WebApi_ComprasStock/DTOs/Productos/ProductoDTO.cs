using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.DTOs.ProveedoresDTOs;
using WebApi_ComprasStock.Entidades;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        public string CodigoBarra { get; set; }

        public string Descripcion { get; set; }

        public int UnidadMedida { get; set; }

        public string Imagen { get; set; }

        public bool Borrado { get; set; }

        public int TipoProductoId { get; set; }

    }
}
