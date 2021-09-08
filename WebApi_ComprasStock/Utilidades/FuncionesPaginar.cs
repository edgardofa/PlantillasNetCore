using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.DTOs;

namespace WebApi_ComprasStock.Utilidades
{
    public class FuncionesPaginar
    {
        //----------------------------------------------------------------------------------------------
        public static List<T> PaginarListas<T>(List<T> lista, PaginacionDTO paginacionDTO)
        {
            int cantReg = lista.Count();
            paginacionDTO.Pagina = paginacionDTO.Pagina < 1 ? 1 : paginacionDTO.Pagina;
            var listaResult = lista.Skip(ControlNumeroPagina(cantReg, paginacionDTO.Pagina, paginacionDTO.RecordsPorPagina)).Take(paginacionDTO.RecordsPorPagina).ToList();
            return listaResult;
        }
        //----------------------------------------------------------------------------------------------
        private static int ControlNumeroPagina(int cantRegistros, int paginaActual, int cantItems)
        {
            int control = paginaActual * cantItems;
            int resultado = 0;
            if (cantRegistros > control)
            {
                paginaActual--;
            }
            else
            {
                paginaActual = (int)(cantRegistros / cantItems);
            }
            resultado = paginaActual * cantItems;
            return resultado;
        }
        //..........................................................................
        private static Func<int, int, int, int> delegado = ControlNumeroPagina;
    }
}
