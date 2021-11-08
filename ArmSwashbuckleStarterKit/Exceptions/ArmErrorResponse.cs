// -----------------------------------------------------------------------
// <copyright file="ArmErrorResponse.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Exceptions
{
    using System;
    using Newtonsoft.Json;

    // Common error response for all Azure Resource Manager APIs to return error details for failed operations. (This also follows the OData error response format.)."
    // https://github.com/Azure/azure-rest-api-specs-pr/blob/375f9743a4b2ee598596c72a456653d747d9fa78/specification/common-types/resource-management/v3/types.json#L331
    public class ArmErrorResponse
    {
        // The error object
        [JsonProperty(PropertyName = "error")]
        public ArmErrorDetail Error;
    }
}
