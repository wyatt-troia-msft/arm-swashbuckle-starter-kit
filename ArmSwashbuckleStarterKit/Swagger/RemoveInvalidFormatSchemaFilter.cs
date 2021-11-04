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
    /// Remove formats not allowed by ARM
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
