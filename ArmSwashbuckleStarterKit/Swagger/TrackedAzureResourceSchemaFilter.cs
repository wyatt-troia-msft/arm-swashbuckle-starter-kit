// -----------------------------------------------------------------------
// <copyright file="XmsAzureResourceSchemaFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Collections.Generic;
    using Microsoft.ArmSwashbuckleStarterKit.Attributes;

    /// <summary>
    /// Adds x-ms-azure-resource extension to schemas marked with <see cref="SwaggerIsTrackedARMResourceAttribute"/>. Also adds reference to 
    /// common ARM types for TrackedResource and SystemData.
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r2062-xmsresourceinputresponse">Docs</see>
    /// </summary>
    public class TrackedAzureResourceSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var attributes = context.Type.GetCustomAttributes(typeof(SwaggerIsTrackedARMResourceAttribute), false);
            if (attributes.Length > 0)
            {
                // Add reference to common TrackedResource type
                schema.Extensions.Add("x-ms-azure-resource", new OpenApiBoolean(true));

                schema.AllOf = new List<OpenApiSchema>
                {
                    new OpenApiSchema
                    {
                        Reference = new OpenApiReference { Id = $"definitions/{SwaggerConstants.TrackedResource}", ExternalResource = SwaggerConstants.CommonTypesV3 }
                    }
                };

                var trackedResourcePropertyKeys = new List<string> { "tags", "location", "id", "name", "type", "systemData" };

                foreach (var key in trackedResourcePropertyKeys)
                {
                    if (schema.Properties.ContainsKey(key))
                    {
                        schema.Properties.Remove(key);
                    }
                }

                // Add required systemData property, which for some reason is not detected automatically by the ARM validator
                // even though it is a property of the Resource common type, which the TrackedResource common type inherits from
                var systemDataSchema = new OpenApiSchema { Reference = new OpenApiReference { Id = $"definitions/{SwaggerConstants.SystemData}", ExternalResource = SwaggerConstants.CommonTypesV3 } };
                schema.Properties.Add("systemData", systemDataSchema);
            }
        }
    }
}
