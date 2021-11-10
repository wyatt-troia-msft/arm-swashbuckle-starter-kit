// -----------------------------------------------------------------------
// <copyright file="ExternalizeDocumentFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Reference shared definitions where available.
    /// </summary>
    public class ExternalizeDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var operations = swaggerDoc.Paths.Values
                .SelectMany(path => path.Operations.Values);

            foreach (var operation in operations)
            {
                var parameters = new List<OpenApiParameter>();
                foreach (var parameter in operation.Parameters)
                {
                    parameters.Add(Translate(parameter) ?? parameter);
                }

                operation.Parameters = parameters;

                foreach (var (responseSchemaName, responseSchemas) in operation.Responses
                    .Select(response => (response.Key, response.Value.Content.FirstOrDefault().Value?.Schema)))
                {
                    this.DeepTranslate(responseSchemaName, responseSchemas);
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Reference = new OpenApiReference { Id = $"parameters/{SwaggerConstants.ApiVersionParameter}", ExternalResource = SwaggerConstants.CommonTypesV3 }
                });
            }

            foreach (var (schemaName, schema) in swaggerDoc.Components.Schemas)
            {
                this.DeepTranslate(schemaName, schema);
            }
        }

        private static OpenApiReference Translate(OpenApiReference reference)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (reference.Id)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
                case "SystemDataModel":
                    return new OpenApiReference { Id = $"definitions/{SwaggerConstants.SystemData}", ExternalResource = SwaggerConstants.CommonTypesV3 };
                case "UserAssignedIdentity":
                    return new OpenApiReference { Id = $"definitions/{SwaggerConstants.UserAssignedIdentity}", ExternalResource = SwaggerConstants.SwaggerFileName };
                case SwaggerConstants.MockResource:
                    return new OpenApiReference { Id = $"definitions/{SwaggerConstants.Resource}", ExternalResource = SwaggerConstants.CommonTypesV3 };
                case SwaggerConstants.MockTrackedResource:
                    return new OpenApiReference { Id = $"definitions/{SwaggerConstants.TrackedResource}", ExternalResource = SwaggerConstants.CommonTypesV3 };
                case "ErrorResponse":
                    return new OpenApiReference { Id = $"definitions/{SwaggerConstants.ErrorResponse}", ExternalResource = SwaggerConstants.SwaggerFileName };
                default:
                    return null;
            }
        }

        private static OpenApiParameter Translate(OpenApiParameter opParam)
        {
            if (opParam.In == ParameterLocation.Path && opParam.Schema.Type == "string")
            {
                switch (opParam.Name)
                {
                    case "subscriptionId":
                        return new OpenApiParameter
                        {
                            Reference = new OpenApiReference { Id = $"parameters/{SwaggerConstants.SubscriptionIdParameter}", ExternalResource = SwaggerConstants.CommonTypesV3 }
                        };
                    case "resourceGroupName":
                        return new OpenApiParameter
                        {
                            Reference = new OpenApiReference { Id = $"parameters/{SwaggerConstants.ResourceGroupNameParameter}", ExternalResource = SwaggerConstants.CommonTypesV3 }
                        };
                    case "systemData":
                        return new OpenApiParameter
                        {
                            Reference = new OpenApiReference { Id = $"parameters/{SwaggerConstants.SystemData}", ExternalResource = SwaggerConstants.SwaggerFileName }
                        };
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively drill into a schema and update any references
        /// </summary>
        private OpenApiSchema DeepTranslate(string schemaName, OpenApiSchema schema)
        {
            if (schema?.Reference != null)
            {
                schema.Reference = Translate(schema.Reference) ?? schema.Reference;
                if (schema.Reference.Id == $"definitions/{SwaggerConstants.SystemData}")
                {
                    schema.ReadOnly = true;
                }

                return schema;
            }

            if (schema?.AllOf?.Count != 0)
            {
                for (int i = 0; i < schema?.AllOf?.Count; i++)
                {
                    schema.AllOf[i] = this.DeepTranslate(schemaName, schema.AllOf[i]);
                }
            }

            if (schema != null && schema?.Properties?.Count != 0)
            {
                foreach (var innerSchema in schema?.Properties)
                {
                    schema.Properties[innerSchema.Key] = this.DeepTranslate(innerSchema.Key, innerSchema.Value) ?? innerSchema.Value;
                }
            }

            return schema;
        }
    }
}
