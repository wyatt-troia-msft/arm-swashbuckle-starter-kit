// -----------------------------------------------------------------------
// <copyright file="DeleteProducesOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// A bit of a hack. ARM prohibits returning a response body for DELETEs, but its auto-validator errors if a delete 
    /// operation Swagger section doesn't include a "produces" property. Swashbuckle, for its part, won't generate the "produce" 
    /// property unless the controller method's "ProducesResponseType" attribute is provided a response body type.
    /// The solve used by this repo is to use both "ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)" 
    /// and "Produces("application/json")" attributes on a delete controller method and then to use this DeleteProducesOperationFilter
    /// to remove the dummy response body type, resulting the "produces" Swagger section being generated while avoiding
    /// adding any response body schema.
    /// <see href="https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#r3013-deletemustnothaverequestbody">Docs</see>
    /// </summary>
    public class DeleteProducesOperationFilter : IOperationFilter
    {
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
