using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Validaciones;

namespace WebApi_ComprasStock.Entidades
{
    public class Producto:IImplementsId
    {
        [Key]
        public int Id { get; set; }

        [StringLength(maximumLength:13,MinimumLength =13
            ,ErrorMessage = "El campo {0} no debe tener más de {1} carácteres y al menos {2} carateres")]
        public string CodigoBarra { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 100, MinimumLength = 5,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres y al menos {2} carateres")]
        [PrimeraLetraMayuscula]
        public string Descripcion { get; set; }


        public string Imagen { get; set; }

        public bool Borrado { get; set; }

        public int TipoProductoId { get; set; }
        [ForeignKey("TipoProductoId")]
        public TipoProducto TipoProducto { get; set; }

        public List<ProveedorProducto> ProveedorProducto { get; set; }

        public int UnidadMedida { get; set; }
        [ForeignKey("UnidadMedida")]
        public UnidadDeMedida UnidadDeMedida { get; set; }
    }
}
