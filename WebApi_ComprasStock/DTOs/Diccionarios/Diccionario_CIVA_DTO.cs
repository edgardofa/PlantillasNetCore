using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs.Diccionarios
{
    public class Diccionario_CIVA_DTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Activado { get; set; }
    }
}
