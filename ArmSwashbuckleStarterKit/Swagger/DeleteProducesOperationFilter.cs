// -----------------------------------------------------------------------
// <copyright file="DeleteProducesOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class DeleteProducesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// add x-ms-pageable extension per Azure guidelines
        /// see https://armwiki.azurewebsites.net/api_contracts/guidelines/openapi.html#oapi009-always-add-x-ms-pageable-to-list-calls
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod == "DELETE")
            {
                foreach (var response in operation.Responses)
                {
                    response.Value.Reference = null; // remove any schema as deletes should not return any response body
                    foreach (var mediaType in response.Value.Content)
                    {
                        mediaType.Value.Schema = null;
                    }
                }

                foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
                {
                    responseType.Type = null;
                    responseType.ModelMetadata = null;
                }
            }
        }
    }
}
