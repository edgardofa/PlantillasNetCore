using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Validaciones
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] tiposAceptados;

        public TipoArchivoValidacion(string [] tiposAceptados)
        {
            this.tiposAceptados = tiposAceptados;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {
            if(grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tiposAceptados = new string[] { "image/jpeg", "image/png", "image.gif" };
            }
        }
        //-------------------------------------------------------------------------------------
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) { return ValidationResult.Success; }
            IFormFile formFile = value as IFormFile;
            if (formFile == null) { return ValidationResult.Success; }

            if (!tiposAceptados.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe ser uno de los siguientes: {string.Join(", ", tiposAceptados)}");
            }

            return ValidationResult.Success; ;
        }
    }
}
