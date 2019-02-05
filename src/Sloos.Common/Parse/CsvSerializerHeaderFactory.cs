// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Sloos.Common
{
    public sealed class CsvSerializerHeaderFactory
    {
        // TODO(chrboum): why is this never used?
        private static readonly Dictionary<Type, string> TypeMap;
        private readonly Type type;

        static CsvSerializerHeaderFactory()
        {
            CsvSerializerHeaderFactory.TypeMap = new Dictionary<Type, string>()
            {
                // value types
                { typeof(byte), "byte" },
                { typeof(sbyte), "sbyte" },
                { typeof(char), "char" },
                { typeof(short), "short" },
                { typeof(ushort), "ushort" },
                { typeof(int), "int" },
                { typeof(uint), "uint" },
                { typeof(long), "long" },
                { typeof(ulong), "ulong" },
                { typeof(float), "float" },
                { typeof(double), "double" },
                { typeof(decimal), "decimal" },
                { typeof(Guid), "Guid" },
                { typeof(TimeSpan), "TimeSpan" },
                { typeof(DateTime), "DateTime" },
                { typeof(DateTimeOffset), "DateTimeOffset" },

                // nullable types
                { typeof(byte?), "byte?" },
                { typeof(sbyte?), "sbyte?" },
                { typeof(char?), "char?" },
                { typeof(short?), "short?" },
                { typeof(ushort?), "ushort?" },
                { typeof(int?), "int?" },
                { typeof(uint?), "uint?" },
                { typeof(long?), "long?" },
                { typeof(ulong?), "ulong?" },
                { typeof(float?), "float?" },
                { typeof(double?), "double?" },
                { typeof(decimal?), "decimal?" },
                { typeof(Guid?), "Guid?" },
                { typeof(TimeSpan?), "TimeSpan?" },
                { typeof(DateTime?), "DateTime?" },
                { typeof(DateTimeOffset?), "DateTimeOffset?" },

                // reference type
                { typeof(string), "string" },
            };
        }

        public CsvSerializerHeaderFactory(Type type)
        {
            this.type = type;
        }

        public IEnumerable<DelimitedColumn> GetDelimitedColumns()
        {
            foreach (var propertyInfo in CsvSerializerHelpers.OrderByProperty(this.type))
            {
                var delimitedColumn = new DelimitedColumn()
                {
                    Name = propertyInfo.Name,
                    Type = propertyInfo.PropertyType,
                };

                yield return delimitedColumn;
            }
        }
    }
}
