// -----------------------------------------------------------------------
// <copyright file="PageableOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.ArmSwashbuckleStarterKit;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// add x-ms-pageable extension per Azure guidelines.
    /// <see href="https://armwiki.azurewebsites.net/api_contracts/guidelines/openapi.html#oapi009-always-add-x-ms-pageable-to-list-calls">Docs</see>
    /// </summary>
    public class PageableOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.ReturnType.FullName.Contains(typeof(ResourceListResultModel<>).FullName))
            {

                operation.Extensions.Add(
                    "x-ms-pageable",
                    new OpenApiObject
                    {
                        ["nextLinkName"] = new OpenApiString("nextLink")
                    });
            }
        }
    }
}
