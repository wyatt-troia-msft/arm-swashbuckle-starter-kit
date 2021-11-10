// -----------------------------------------------------------------------
// <copyright file="XmsLongRunningOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Linq;
    using Microsoft.ArmSwashbuckleStarterKit.Attributes;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds x-ms-long-running-operation extension to operation marked with <see cref="SwaggerLongRunningOperationAttribute"/>.
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r2007-longrunningoperationswithlongrunningextension">Docs</see>
    /// </summary>
    public class XmsLongRunningOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // A 'POST' operation with x-ms-long-running-operation extension must have a valid terminal success status code 200 or 201 or 204.
            // API that responds only 202 cannot marked as `x-ms-long-running-operation`. Make sure API responds 202 also responds 200 and/or 201 and/or 204.
            var longRunningOperationAttributes = context.ApiDescription.CustomAttributes().OfType<SwaggerLongRunningOperationAttribute>();
            if (longRunningOperationAttributes.Any())
            {
                operation.Extensions.Add("x-ms-long-running-operation", new OpenApiBoolean(true));
                operation.Extensions.Add("x-ms-long-running-operation-options", new OpenApiObject
                {
                    // ARM requires using 'Location' header to provide URL to poll
                    // https://github.com/Azure/azure-resource-manager-rpc/blob/master/v1.0/async-api-reference.md#delete-resource-asynchronously
                    // https://github.com/Azure/autorest/blob/main/docs/extensions/readme.md#x-ms-long-running-operation-options
                    { "final-state-via",  new OpenApiString("location") },
                });
            }
        }
    }
}
