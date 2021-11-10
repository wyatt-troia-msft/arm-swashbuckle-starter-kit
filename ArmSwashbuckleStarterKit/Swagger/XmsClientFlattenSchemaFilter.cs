// -----------------------------------------------------------------------
// <copyright file="XmsClientFlattenSchemaFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Interfaces;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Collections.Generic;

    /// <summary>
    /// Adds x-ms-client-flatten extension to schemas with nested properties.
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r2001-avoidnestedproperties">Docs</see>
    /// </summary>
    public class XmsClientFlattenSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
            {
                return;
            }

            foreach (var prop in schema.Properties)
            {
                if (prop.Key == "properties" && prop.Value.Reference != null)
                {
                    prop.Value.Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        { "x-ms-client-flatten", new OpenApiBoolean(true) },
                        { "$ref", new OpenApiString(prop.Value.Reference.ReferenceV2) }
                    };
                    prop.Value.Reference = null; // prevent the reference from overwriting the extensions
                }
            }
        }
    }
}
