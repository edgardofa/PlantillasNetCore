using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccesor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccesor)
        {
            this.env = env;
            this.httpContextAccesor = httpContextAccesor;
        }
        //____________________________________________________________________________________________________________
        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta)) { return Task.CompletedTask; }

            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(env.WebRootPath, contenedor, nombreArchivo);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.FromResult(0);
        }

        //____________________________________________________________________________________________________________
        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string contentType, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        //____________________________________________________________________________________________________________
        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var nombreArchivo = $"{ Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = $"{httpContextAccesor.HttpContext.Request.Scheme}://{httpContextAccesor.HttpContext.Request.Host}";
            var rutaParaDB = Path.Combine(urlActual, contenedor, nombreArchivo)
                .Replace("\\", "/");

            return rutaParaDB;
        }
        //____________________________________________________________________________________________________________
    }
}
