using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class CategoriaDTOCompleta: CategoriaDTO
    {
        public List<TipoProductoDTO> TipoProductos { get; set; }

        public RubroDTO Rubro { get; set; }
    }
}
