using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        public readonly int cantidadMaximaRecordsPorPagina = 50;

        public string NombreCampoOrden { get; set; }
        public bool OrdenAscendente { get; set; } = true;
        public string CampoThenBy { get; set; } = string.Empty;
        public bool OrdenThenBy { get; set; } = true;


        private int recordsPorPagina = 10;
        

        public int RecordsPorPagina
        {
            get { return recordsPorPagina; }
            set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }
    }
}
