// -----------------------------------------------------------------------
// <copyright file="ArmErrorDetail.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Exceptions
{
    using Newtonsoft.Json;

    // The common ARM type for error detail
    // https://github.com/Azure/azure-rest-api-specs-pr/blob/375f9743a4b2ee598596c72a456653d747d9fa78/specification/common-types/resource-management/v3/types.json#L294
    public class ArmErrorDetail
    {
        // The error code
        [JsonProperty(PropertyName = "code")]
        public string Code;

        // The error message
        [JsonProperty(PropertyName = "message")]
        public string Message;

        // The error target
        [JsonProperty(PropertyName = "target")]
        public string Target;

        // The error details
        [JsonProperty(PropertyName = "details")]
        public ArmErrorDetail[] Details;

        // The error additional info
        [JsonProperty(PropertyName = "additionalInfo")]
        public string AdditionalInfo;
    }
}
