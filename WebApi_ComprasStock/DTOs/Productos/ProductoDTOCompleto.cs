using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.DTOs.ProveedoresDTOs;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class ProductoDTOCompleto: ProductoDTO
    {
        public List<ProveedorDTO> Proveedores { get; set; }
        public TipoProductoDTO TipoProducto { get; set; }
    }
}
