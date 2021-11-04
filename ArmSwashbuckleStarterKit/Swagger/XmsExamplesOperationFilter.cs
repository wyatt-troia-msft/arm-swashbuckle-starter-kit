// -----------------------------------------------------------------------
// <copyright file="XmsExamplesOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Generate paths to example files
    /// </summary>
    /// <remarks>The example files themselves are hard-coded/>.</remarks>
    public class XmsExamplesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var examples = new OpenApiObject();

            foreach (var (title, filePath) in SwaggerExtensions.GetSwaggerExampleReferences(context.MethodInfo))
            {
                examples[title] = new OpenApiObject { ["$ref"] = new OpenApiString(filePath) };
            }

            if (examples.Count > 0)
            {
                operation.Extensions.Add("x-ms-examples", examples);
            }
        }
    }
}
