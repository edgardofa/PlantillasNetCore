using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task BorrarArchivo(string ruta, string contenedor);
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, 
            string contentType, string ruta);
        Task<string> GuardarArchivo(byte[] contenido,string extension, string contenedor, string contentType);
    }
}
