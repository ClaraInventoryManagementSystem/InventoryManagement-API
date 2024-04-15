using InventoryManagement.Handlers;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement
{
    public static class Util
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler>();
        }


        //public static IApplicationBuilder UseAuthenticationHandlingMiddleware(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<FLEXIAuthenticationHandler>();
        //}
    }

    public class AppSettings
    {
        public static string ConnStringName { get; set; }

        //public string StorageConnectionString { get; set; }

        //public string AzureStorageAccountContainer { get; set; }
    }
}
