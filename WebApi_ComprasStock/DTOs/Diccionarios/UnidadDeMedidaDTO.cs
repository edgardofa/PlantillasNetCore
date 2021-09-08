using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs.Diccionarios
{
    public class UnidadDeMedidaDTO
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }
    }
}
