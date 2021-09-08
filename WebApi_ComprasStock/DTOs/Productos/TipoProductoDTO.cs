using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class TipoProductoDTO
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string CodigoTipo { get; set; }
    }
}
