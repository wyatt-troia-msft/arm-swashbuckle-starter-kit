// -----------------------------------------------------------------------
// <copyright file="SwaggerLinkToExampleAttribute.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Attributes
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Used to mark a controller method whose Swagger operation section will require a link to a file
    /// containing example requests and responses.The attribute sets the controller method name as the
    /// name of the example file by default, but you can also pass a custom filename in as a parameter.
    /// Processing of these attributes is done by ../Swagger/XmsExamplesOperationFilter.cs
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class SwaggerLinkToExampleAttribute : Attribute
    {
        public SwaggerLinkToExampleAttribute([CallerMemberName] string opName = null)
        {
            this.OpName = opName;
        }

        public string OpName { get; set; }
    }
}
