// -----------------------------------------------------------------------
// <copyright file="SwaggerConstants.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    /// <summary>
    /// Constants used by Swashbuckle filters.
    /// </summary>
    public static class SwaggerConstants
    {
        public const string SwaggerFileName = "edgezones.json";

        // External swagger resources
        public const string ExampleBasePath = "./examples";

        public const string WorkspaceNameParameter = "WorkspaceNameParameter";

        public const string UserAssignedIdentity = "UserAssignedIdentity";

        public const string ContainerResourceRequirements = "ContainerResourceRequirements";

        public const string LivenessProbeRequirements = "LivenessProbeRequirements";

        // Belonging to transitional references
        public const string MockResource = "MockResource";

        public const string MockTrackedResource = "MockTrackedResource";

        // Belonging to Common Types V3
        public const string CommonTypesV3 = "../../../../../common-types/resource-management/v3/types.json";

        public const string SystemData = "systemData";

        public const string SubscriptionIdParameter = "SubscriptionIdParameter";

        public const string ResourceGroupNameParameter = "ResourceGroupNameParameter";

        public const string ApiVersionParameter = "ApiVersionParameter";

        public const string Resource = "Resource";

        public const string TrackedResource = "TrackedResource";

        public const string ErrorResponse = "ErrorResponse";
    }
}
