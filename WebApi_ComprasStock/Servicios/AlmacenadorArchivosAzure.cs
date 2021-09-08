﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Servicios
{
    public class AlmacenadorArchivosAzure : IAlmacenadorArchivos
    {
        private readonly string connectionString;
        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }
        //-----------------------------------------------------------------------------
        public async Task BorrarArchivo(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta)) { return; }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }
        //____________________________________________________________________________________________________________
        public async Task<string> EditarArchivo(byte[] contenido, string extension, 
            string contenedor, string contentType, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }
        //____________________________________________________________________________________________________________
        public async Task<string> GuardarArchivo(byte[] contenido, string extension, 
            string contenedor, string contentType)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var archivoNombre = $"{Guid.NewGuid()} {extension}";
            var blob = cliente.GetBlobClient(archivoNombre);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeaders;


            await blob.UploadAsync(new BinaryData(contenido),blobUploadOptions);

            return blob.Uri.ToString();
        }
    }
}