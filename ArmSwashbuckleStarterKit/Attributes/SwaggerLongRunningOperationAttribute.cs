// -----------------------------------------------------------------------
// <copyright file="SwaggerLongRunningOperationAttribute.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Attributes
{
    using System;

    /// <summary>
    /// Used to mark controller methods that may run asynchronously
    /// </summary>
    public class SwaggerLongRunningOperationAttribute : Attribute
    {
    }
}
