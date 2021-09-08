using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Validaciones;

namespace WebApi_ComprasStock.DTOs.Productos
{
    public class TipoProductoCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 75, MinimumLength = 3,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres y al menos {2} carateres")]
        [PrimeraLetraMayuscula]
        public string Descripcion { get; set; }

        [StringLength(maximumLength: 7, MinimumLength = 7)]
        public string CodigoTipo { get; set; }
        
        [Required]
        public int CategoriasId { get; set; }
    }
}
