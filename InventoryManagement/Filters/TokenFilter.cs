﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InventoryManagement.Filters
{
    public class TokenFilter : IOperationFilter
    {


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "TokenKey",
                In = ParameterLocation.Header,
                Required = false // set to false if this is optional
            });
        }
    }
}
