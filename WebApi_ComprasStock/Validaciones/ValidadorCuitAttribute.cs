using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Validaciones
{
    public class ValidadorCuitAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult resultado = ValidationResult.Success;
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return resultado;
            }

            string datoCuit = value.ToString();
            Int64 control = 0;
            bool res = Int64.TryParse(datoCuit, out control);
            if (!res) 
            {
                return new ValidationResult("C.U.I.T inválido");
            }
            //.......................................................
            try
            {
                // Carga de vector con ls digitos verificadores
                int[] vect = new int[10];
                int iAux = 2;
                for (int i = 9; i > 3; i--)
                {
                    vect[i] = iAux;
                    if (i > 5) { vect[i - 6] = iAux; }
                    iAux++;
                }
                // Proceso verificador
                Int32 iSuma = 0;
                for (int i = 0; i < 10; i++)
                {
                    string dig = datoCuit.Substring(i, 1);
                    Int32 digit = 0;
                    bool result = Int32.TryParse(dig, out digit);
                    iAux = vect[i] * digit;
                    iSuma = iSuma + iAux;
                }
                int resto = iSuma % 11;
                string sDig = datoCuit.Substring(10, 1);
                Int32 digito = 0;
                bool resulta = Int32.TryParse(sDig, out digito);
                switch (resto)
                {
                    case 0:
                        if (resto == digito)
                        {
                            resultado= ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult("C.U.I.T inválido");
                        }
                        break;
                    case 1:
                        return new ValidationResult("C.U.I.T inválido");
                    default:
                        resto = 11 - resto;
                        if (resto == digito)
                        {
                            resultado = ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult("C.U.I.T inválido");
                        }
                        break;
                }
                return resultado;
            }
            catch (Exception)
            {
                return new ValidationResult("C.U.I.T inválido");
            }

        }




}
}
