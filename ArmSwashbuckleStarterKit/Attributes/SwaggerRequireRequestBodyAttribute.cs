// -----------------------------------------------------------------------
// <copyright file="SwaggerRequireRequestBodyAttribute.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Attributes
{
    using System;

    /// <summary>
    /// Used to mark controller methods that should be excluded from the operations in the Swagger spec
    /// </summary>
    public class SwaggerRequireRequestBodyAttribute : Attribute
    {
    }
}
