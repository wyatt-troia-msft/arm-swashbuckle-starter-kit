// -----------------------------------------------------------------------
// <copyright file="ArmErrorAdditionalInfo.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Exceptions
{
    using Newtonsoft.Json;

    // The common ARM type for resource management error additional info
    // https://github.com/Azure/azure-rest-api-specs-pr/blob/375f9743a4b2ee598596c72a456653d747d9fa78/specification/common-types/resource-management/v3/types.json#L342
    public class ArmErrorAdditionalInfo
    {
        // The additional info type
        [JsonProperty(PropertyName = "type")]
        public string Type;

        // The additional info
        [JsonProperty(PropertyName = "info")]
        public object Info;
    }
}
