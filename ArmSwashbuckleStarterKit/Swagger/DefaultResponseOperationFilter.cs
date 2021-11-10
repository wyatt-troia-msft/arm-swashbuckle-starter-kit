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

    /// <summary>
    /// Adds a default error response for each operation, referencing the ARM common type.
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r4010-requireddefaultresponse">Docs 1</see>
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r4007-defaulterrorresponseschema">Docs 2</see>
    /// <see href="https://github.com/Azure/azure-rest-api-specs-pr/blob/375f9743a4b2ee598596c72a456653d747d9fa78/specification/common-types/resource-management/v3/types.json#L331">Docs 3</see>
    /// </summary>
    public class DefaultResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var pathToCommonArmErrorResponseType = $"{SwaggerConstants.CommonTypesV3}#/definitions/{SwaggerConstants.ErrorResponse}";
            operation.Responses.Add("default", new OpenApiResponse
            {
                Description = "Error response describing why the operation failed.",
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {
                        "schema", new OpenApiObject { ["$ref"] = new OpenApiString(pathToCommonArmErrorResponseType) }
                    }
                },
            });
        }
    }
}
