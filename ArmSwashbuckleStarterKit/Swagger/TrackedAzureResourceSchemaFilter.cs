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
    /// Operation Filter to satisfy ARM API Rule 2062
    /// </summary>
    /// <remarks>
    /// this will correct the following AutoRest error:<br />
    /// ERROR(XmsResourceInPutResponse/R2062/ARMViolation) : The 200 response model for an ARM PUT operation must have x-ms-azure-resource extension set to true in its hierarchy.
    /// </remarks>
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
