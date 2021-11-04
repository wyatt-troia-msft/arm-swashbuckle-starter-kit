// -----------------------------------------------------------------------
// <copyright file="ExcludeSchemaFilter.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.ArmSwashbuckleStarterKit.Attributes;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ExcludeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
        {
            if (schema.Properties.Count == 0)
            {
                return;
            }

            const BindingFlags bindingFlags = BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance;
            var memberList = schemaFilterContext.Type
                                .GetFields(bindingFlags).Cast<MemberInfo>()
                                .Concat(schemaFilterContext.Type
                                .GetProperties(bindingFlags));

            var excludedList = memberList.Where(m =>
                                                m.GetCustomAttribute<SwaggerExcludeAttribute>()
                                                != null)
                                         .Select(m =>
                                             (m.GetCustomAttribute<JsonPropertyAttribute>()
                                              ?.PropertyName
                                              ?? m.Name.ToCamelCase()));

            foreach (var excludedName in excludedList)
            {
                if (schema.Properties.ContainsKey(excludedName))
                {
                    schema.Properties.Remove(excludedName);
                }
            }
        }
    }
}
