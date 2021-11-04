//-----------------------------------------------------------------------
// <copyright file="ResourceListResultModel.cs" company="Microsoft Corp.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a list of resources
    /// </summary>
    public class ResourceListResultModel<T>
    {
        /// <summary>
        /// A link to the next page of results
        /// </summary>
        [JsonProperty("nextLink")]
        public string NextLink { get; set; }

        /// <summary>
        /// The current page of results
        /// </summary>
        [JsonProperty("value")]
        public T[] Value { get; set; }
    }
}
