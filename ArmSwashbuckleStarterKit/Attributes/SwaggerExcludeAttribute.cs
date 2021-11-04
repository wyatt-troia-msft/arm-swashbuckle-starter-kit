// -----------------------------------------------------------------------
// <copyright file="SwaggerExcludeAttribute.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Attributes
{
    using System;

    /// <summary>
    /// Used to mark properties to be excluded from a Swagger schema by ../Swagger/ExcludeSchemaFilter.cs
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}
