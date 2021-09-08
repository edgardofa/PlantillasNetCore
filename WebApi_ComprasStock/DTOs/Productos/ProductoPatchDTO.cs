using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Validaciones;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class ProductoPatchDTO
    {
        [StringLength(maximumLength: 13, MinimumLength = 13
            , ErrorMessage = "El campo {0} no debe tener más de {1} carácteres y al menos {2} carateres")]
        public string CodigoBarra { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 100, MinimumLength = 5,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres y al menos {2} carateres")]
        [PrimeraLetraMayuscula]
        public string Descripcion { get; set; }

        public bool Borrado { get; set; }

        public int TipoProductoId { get; set; }

        public int UnidadMedida { get; set; }
    }
}
