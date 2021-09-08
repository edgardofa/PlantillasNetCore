using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class TipoProductoDTOCompleta: TipoProductoDTO
    {
        public List<ProductoDTO> Productos { get; set; }
        public CategoriaDTO Categoria { get; set; }
    }
}
