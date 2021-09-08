using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Validaciones;

namespace WebApi_ComprasStock.Entidades
{
    public class DatosProveedores : IImplementsId
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 100, MinimumLength = 3,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres y al menos {2} carateres")]
        [PrimeraLetraMayuscula]
        public string RSocial { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 11, MinimumLength = 11,
            ErrorMessage = "El campo {0} debe tener {1} dígitos sin guiones ni separación")]
        [ValidadorCuit]
        public string Cuit { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(maximumLength: 11, MinimumLength = 11,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        public string Web { get; set; }

        [Required]
        [StringLength(maximumLength: 100,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        public string Direccion { get; set; }

        [Required]
        [StringLength(maximumLength: 100,
            ErrorMessage = "El campo {0} no debe tener más de {1} carácteres")]
        public string Telefonos { get; set; }

        [Required]
        public int CIVAId { get; set; }
        [ForeignKey("CIVAId")]

        public Diccionario_CIVA Diccionario_CIVA { get; set; }

        public bool Activo { get; set; }

        public List<ProveedorProducto> ProveedorProducto { get; set; }

    }
}
