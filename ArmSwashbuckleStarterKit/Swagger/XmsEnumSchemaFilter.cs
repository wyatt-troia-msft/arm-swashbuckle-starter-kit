// -----------------------------------------------------------------------
// <copyright file="XmsEnumSchemaFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class XmsEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // when generating code serialize all enums as strings for future back-compat
            // see https://github.com/Azure/autorest/blob/main/docs/extensions/readme.md#x-ms-enum
            Type type = context.Type;
            if (type.IsEnum)
            {
                schema.Extensions.Add(
                    "x-ms-enum",
                    new OpenApiObject
                    {
                        ["name"] = new OpenApiString(type.Name),
                        ["modelAsString"] = new OpenApiBoolean(true)
                    }
                );
            };
        }
    }
}
