//-----------------------------------------------------------------------
// <copyright file="ExceptionHandlerMiddleware.cs" company="Microsoft Corp.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.ArmSwashbuckleStarterKit.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class ExceptionHandlerMiddleware
    {
        private static readonly string InternalErrorMessage = "Please contact Microsoft EdgeRP development team at edgerp@microsoft.com";

        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception e)
            {
                var armErrorDetail = new ArmErrorDetail
                {
                    Code = "500", // TODO: derive status code from exception
                    Message = InternalErrorMessage,
                    Details = new ArmErrorDetail[]
                    {
                        new ArmErrorDetail
                        {
                            Code = "500",
                            Message = e.Message,
                            Target = e.TargetSite.ToString()
                        }
                    }
                };
                var errorResponse = new ArmErrorResponse { Error = armErrorDetail };

                this.SendErrorResponse(context, errorResponse, 500);
            }
        }

        private async void SendErrorResponse(HttpContext context, ArmErrorResponse errorResponse, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
