// -----------------------------------------------------------------------
// <copyright file="RequireRequestBodyOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Linq;
    using Microsoft.ArmSwashbuckleStarterKit.Attributes;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Mark the request body as required on operations for controller methods decorated with the <see cref="SwaggerRequireRequestBodyAttribute"/>.
    /// </summary>
    public class RequireRequestBodyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attribute = context.MethodInfo.GetCustomAttributes(typeof(SwaggerRequireRequestBodyAttribute), false).FirstOrDefault();
            if (attribute == null || operation.RequestBody == null)
            {
                return;
            }

            operation.RequestBody.Required = true;
        }
    }
}
