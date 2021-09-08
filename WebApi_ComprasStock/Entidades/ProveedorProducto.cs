using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Entidades
{
    public class ProveedorProducto
    {
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }

        public DatosProveedores DatosProveedores { get; set; }
        public Producto Producto { get; set; }
    }
}
