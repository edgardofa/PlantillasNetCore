using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_ComprasStock.Filtros
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger seriLogger;

        public FiltroDeExcepcion(ILogger seriLogger)
        {
            this.seriLogger = seriLogger;
        }

        public override void OnException(ExceptionContext context)
        {
            seriLogger.Error(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}