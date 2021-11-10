// -----------------------------------------------------------------------
// <copyright file="RemoveInvalidFormatSchemaFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Collections.Generic;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Remove formats not allowed by ARM.
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r3017-guidusage">Docs</see>
    /// </summary>
    public class RemoveInvalidFormatSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            foreach (KeyValuePair<string, OpenApiSchema> entry in schema.Properties)
            {
                var propertySchema = entry.Value;
                if (propertySchema.Format == "guid" || propertySchema.Format == "uuid")
                {
                    propertySchema.Format = null;
                }
            }
        }
    }
}
