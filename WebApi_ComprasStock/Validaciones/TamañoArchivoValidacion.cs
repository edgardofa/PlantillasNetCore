using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Validaciones
{
    public class TamañoArchivoValidacion:ValidationAttribute
    {
        private readonly int pesoMaximoEnMB;

        public TamañoArchivoValidacion(int pesoMaximoEnMB)
        {
            this.pesoMaximoEnMB = pesoMaximoEnMB;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null) { return ValidationResult.Success; }
            IFormFile formFile = value as IFormFile;
            if(formFile == null) { return ValidationResult.Success; }

            if(formFile.Length > (pesoMaximoEnMB * 1024 * 1024))
            {
                return new ValidationResult($"El tamaño de la imagen no debe ser mayor a {pesoMaximoEnMB}MB");
            }

            return ValidationResult.Success; ;
        }
    }
}
