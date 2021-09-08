using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class RubroDTOCompleta:RubroDTO
    {
        public List<CategoriaDTO> Categorias { get; set; }
    }
}
