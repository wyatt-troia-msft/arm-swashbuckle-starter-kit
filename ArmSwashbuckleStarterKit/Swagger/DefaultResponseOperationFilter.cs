// -----------------------------------------------------------------------
// <copyright file="DefaultResponseOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Collections.Generic;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Interfaces;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    internal class DefaultResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var pathToCommonArmErrorResponseType = $"{SwaggerConstants.CommonTypesV3}#/definitions/{SwaggerConstants.ErrorResponse}";
            operation.Responses.Add("default", new OpenApiResponse
            {
                Description = "Error response describing why the operation failed.",
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-ms-error-response", new OpenApiBoolean(true) },
                    {
                        "schema", new OpenApiObject { ["$ref"] = new OpenApiString(pathToCommonArmErrorResponseType) }
                    }
                },
            });
        }
    }
}
