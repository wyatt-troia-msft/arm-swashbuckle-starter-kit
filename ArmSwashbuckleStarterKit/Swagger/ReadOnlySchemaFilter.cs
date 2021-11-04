// -----------------------------------------------------------------------
// <copyright file="ReadOnlySchemaFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ReadOnlySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Reference == null)
            {
                return;
            }

            if (schema.Reference.Id == "id" || schema.Reference.Id == "name" || schema.Reference.Id == "type")
            {
                schema.ReadOnly = true;
            }
        }
    }
}
