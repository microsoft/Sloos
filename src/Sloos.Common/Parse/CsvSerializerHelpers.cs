// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sloos.Common
{
    public static class CsvSerializerHelpers
    {
        public static IEnumerable<PropertyInfo> OrderByProperty(Type type)
        {
            var propertyInfos = type.GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(DataMemberAttribute)))
                .Select(x => new { PropertyInfo = x, Order = CsvSerializerHelpers.GetDataMemberOffset(x) })
                .OrderBy(x => x.Order)
                .ThenBy((x => x.PropertyInfo.Name))
                .Select(x => x.PropertyInfo)
                .ToArray();

            return propertyInfos;
        }

        private static int GetDataMemberOffset(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.CustomAttributes.First(x => x.AttributeType == typeof(DataMemberAttribute));

            foreach (var namedArgument in attribute.NamedArguments)
            {
                if (namedArgument.MemberName == "Order")
                {
                    return (int)namedArgument.TypedValue.Value;
                }
            }

            return 0;
        }
    }
}
