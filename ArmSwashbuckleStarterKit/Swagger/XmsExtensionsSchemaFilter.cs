// -----------------------------------------------------------------------
// <copyright file="XmsExtensionsSchemaFilter.cs" company="Microsoft Corp.">
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

    public class XmsExtensionsSchemaFilter : ISchemaFilter
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

        /*
         * commented out code is a starting point if we want to clear the ARM warnings related to missing x-ms-client-flatten or x-ms-enum
         *
        /// <summary>
        /// add x-ms-enum, x-ms-client-flatten extension
        /// </summary>
        ///
        public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
        {

            if (schema.Properties != null)
            {
                //if (schema.Properties.Count > 0)
                //{
                //    property.Value.Extensions.Add("x-ms-client-flatten", new OpenApiBoolean(true));
                //}

                foreach (var property in schema.Properties)
                {
                    if (property.Key.Equals("properties", StringComparison.OrdinalIgnoreCase))
                    {
                        property.Value.Extensions.Add("x-ms-client-flatten", new OpenApiBoolean(true));
                    }

                }

                //foreach (var property in schema.Properties.Where(x => x.Value.Enum != null))
                //{
                //    property.Value.Extensions.Add("x-ms-enum", new OpenApiBoolean(true));
                //}
            }
        }
        */
    }
}
