using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace InventoryManagement.Handlers
{
    public class ExceptionHandler 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;
        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            this._next = next;
            _logger = logger;
        }

        //public async Task Invoke(HttpContext context, IHostingEnvironment env)
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// This can be moved to extension class (gng.common.extension) to re-use any other places.
        /// </summary>
        /// <param name="context"></param>        
        /// <param name="exception"></param>
        /// <returns></returns>
        private  Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //static
            //var code = HttpStatusCode.InternalServerError;
            object respObj = exception.Message;     //"exception mesg";

            //var response = context.Response;
            //var message = "Unhandled error";
            //var code = "00009";
            //var errors = new List<string>();

            //response.ContentType = "application/json";
            //response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // log generic exception message (original error)
            _logger.LogError(exception, exception.Message);

            // Response
            //await response.WriteAsync(JsonConvert.SerializeObject(new Error
            //{
            //    Code = code,
            //    Message = message,
            //    Errors = errors
            //}));
            
            //Do your work
            return context.Response.WriteAsync(JsonConvert.SerializeObject(respObj));
        }
    }
}
