// -----------------------------------------------------------------------
// <copyright file="SwaggerExtensions.cs" company="Microsoft Corp.">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ArmSwashbuckleStarterKit.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.ArmSwashbuckleStarterKit.Attributes;
    using Microsoft.OpenApi.Models;
    using Microsoft.OpenApi.Writers;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension methods used by Swashbuckle filters. Almost certainly includes unused code. This was originally copied from another repo.
    /// </summary>
    public static class SwaggerExtensions
    {
        public static string Uncapitalize(this string the_string)
        {
            if (the_string == null)
            {
                return the_string;
            }
            else if (the_string.Length == 1)
            {
                return the_string.ToLower();
            }
            return (Char.ToLower(the_string[0]) + the_string.Substring(1));
        }

        public static string SerializeAsV2String(this OpenApiSchema schema)
        {
            using var stringWriter = new StringWriter();
            var jsonWriter = new OpenApiJsonWriter(stringWriter);
            schema.SerializeAsV2(jsonWriter);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Gets the first generic argument that appears in a list of inheritances
        /// </summary>
        public static Type GetInheritedGeneric(this Type curType)
        {
            while (curType != typeof(object))
            {
                if (curType.GenericTypeArguments.Count() != 0)
                {
                    return curType.GenericTypeArguments.First();
                }

                curType = curType.BaseType;
            }

            return null;
        }

        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static bool IsSubclassOfRawGeneric(this Type toCheck, string genericName)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (genericName == cur.Name)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Gets the last type in a series of generics
        /// </summary>
        public static Type GetInnerGeneric(this Type type)
        {
            while (type.GenericTypeArguments.Count() != 0)
            {
                type = type.GenericTypeArguments[0];
            }

            return type;
        }

        public static IDictionary<string, OpenApiSchema> GetPropertySchemas(this OpenApiSchema schema)
        {
            if (schema.AllOf.Count() != 0)
            {
                return schema.Properties.Concat(schema.AllOf.SelectMany(schema => schema.GetPropertySchemas()))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            return schema.Properties
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static void SetPropertySchemas(this OpenApiSchema schema, IDictionary<string, OpenApiSchema> propertyBag)
        {
            foreach (var key in schema.Properties.Keys.ToList())
            {
                if (propertyBag.ContainsKey(key))
                {
                    schema.Properties[key] = propertyBag[key];
                    propertyBag.Remove(key);
                }
                else
                {
                    // If the propertyBag does not contain the key, then it was explicitly
                    // removed by the user. Reflect that removal in the original
                    schema.Properties.Remove(key);
                }
            }

            foreach (var innerSchema in schema.AllOf)
            {
                innerSchema.SetPropertySchemas(propertyBag);
            }
        }

        public static string GetOperationId(string inheritedGenericName, string opName)
        {
            return $"{inheritedGenericName}_{opName}";
        }

        public static IEnumerable<(string Title, string FilePath)> GetSwaggerExampleReferences(MethodInfo method)
        {
            foreach (var attribute in method.GetCustomAttributes<SwaggerLinkToExampleAttribute>())
            {
                var title = method.Name;
                var fileName = attribute.OpName;
                if (fileName == null)
                {
                    fileName = title;
                }
                else
                {
                    title = attribute.OpName;
                }

                var path = $"{SwaggerConstants.ExampleBasePath}/{fileName}.json";

                yield return (title, path);
            }
        }
    }
}
