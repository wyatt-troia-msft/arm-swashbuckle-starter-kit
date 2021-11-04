// -----------------------------------------------------------------------
// <copyright file="XmsLongRunningOperationFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Linq;
    using Microsoft.ArmSwashbuckleStarterKit.Attributes;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds x-ms-long-running-operation extenstion to opertaion marked with <see cref="SwaggerLongRunningOperationAttribute"/>.
    /// Some REST operations can take a long time to complete. Although REST is not supposed to be stateful,
    /// some operations are made asynchronous while waiting for the state machine to create the resources,
    /// and will reply before the operation on resources are completed.
    /// When x-ms-long-running-operation is specified, there should also be a x-ms-long-running-operation-options specified.
    /// x-ms-long-running-operation-options: string - one of azure-async-operation or location or original-uri.
    /// </summary>
    /// <see href="https://github.com/Azure/autorest/blob/master/docs/extensions/readme.md#x-ms-long-running-operation">x-ms-long-running-operation.</see>
    public class XmsLongRunningOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies filter.
        /// </summary>
        /// <param name="operation">OpenApiOperation.</param>v
        /// <param name="context">DocumentFilterContext.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // A 'POST' operation with x-ms-long-running-operation extension must have a valid terminal success status code 200 or 201 or 204.
            // API that responds only 202 cannot marked as `x-ms-long-running-operation`. Make sure API responds 202 also responds 200 and/or 201 and/or 204.
            var longRunningOperationAttributes = context.ApiDescription.CustomAttributes().OfType<SwaggerLongRunningOperationAttribute>();
            if (longRunningOperationAttributes.Any())
            {
                operation.Extensions.Add("x-ms-long-running-operation", new OpenApiBoolean(true));
                operation.Extensions.Add("x-ms-long-running-operation-options", new OpenApiObject
                {
                    // ARM requires using 'location' header to provide URL to poll
                    // https://github.com/Azure/azure-resource-manager-rpc/blob/master/v1.0/async-api-reference.md#delete-resource-asynchronously
                    { "final-state-via",  new OpenApiString("location") },
                });
            }
        }
    }
}
