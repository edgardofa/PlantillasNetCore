using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.DTOs.Productos;

namespace WebApi_ComprasStock.DTOs.ProveedoresDTOs
{
    public class ProveedorDTOConProductos: ProveedorDTO
    {
        public List<ProductoDTO> Productos { get; set; }
    }
}
