// -----------------------------------------------------------------------
// <copyright file="XmsErrorOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class XmsErrorOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses != null && operation.Responses.Any())
            {
                foreach (var response in operation.Responses)
                {
                    // For positive responses, leave the schema as is.
                    if (string.Equals(response.Key, "200") ||
                        string.Equals(response.Key, "201") ||
                        string.Equals(response.Key, "202") ||
                        string.Equals(response.Key, "204"))
                    {
                        continue;
                    }

                    // For negative responses, default to the default error response
                    if (response.Value != null)
                    {
                        response.Value.Extensions.TryAdd("x-ms-error-response", new OpenApiBoolean(true));
                    }
                }
            }
        }
    }
}
