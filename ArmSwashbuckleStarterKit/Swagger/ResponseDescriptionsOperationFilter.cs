// -----------------------------------------------------------------------
// <copyright file="ResponseDescriptionsOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ResponseDescriptionsOperationFilter : IOperationFilter
    {
        private static Dictionary<string, string> statusCodeToDescription = new Dictionary<string, string>
        {
            { "200", "OK" },
            { "201", "Created" },
            { "202", "Accepted" },
            { "204", "No Content" },
            { "400", "Bad Request" },
            { "401", "Unauthorized" },
            { "403", "Forbidden" },
            { "404", "Not Found" },
            { "405", "Method Not Allowed" },
            { "406", "Not Acceptable" },
            { "500", "Server Error" },
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses != null && operation.Responses.Any())
            {
                foreach (var response in operation.Responses)
                {
                    if (statusCodeToDescription.ContainsKey(response.Key))
                    {
                        response.Value.Description = statusCodeToDescription[response.Key];
                    }
                }
            }
        }
    }
}
