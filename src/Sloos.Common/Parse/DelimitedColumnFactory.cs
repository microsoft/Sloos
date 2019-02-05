// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sloos.Common
{
    public sealed class DelimitedColumnFactory
    {
        public DelimitedColumn Create(string s)
        {
            var split = this.SplitColumn(s);

            var columnInfo = new DelimitedColumn()
            {
                Name = split.Item1,
                Type = split.Item2,
            };

            return columnInfo;
        }

        public IEnumerable<DelimitedColumn> Create(IEnumerable<string> strings)
        {
            return strings.Select(this.Create);
        }

        private Tuple<string, Type> SplitColumn(string column)
        {
            var a = column.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            return a.Length == 1 ?
                Tuple.Create(a[0], typeof(string)) :
                Tuple.Create(a[0], this.ConvertToType(a[1]));
        }

        private Type ConvertToType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "bool":
                    return typeof(bool);
                case "bool?":
                    return typeof(bool?);
                case "byte":
                    return typeof(byte);
                case "byte?":
                    return typeof(byte?);
                case "char":
                    return typeof(char);
                case "char?":
                    return typeof(char?);
                case "datetime":
                    return typeof(DateTime);
                case "datetime?":
                    return typeof(DateTime?);
                case "datetimeoffset":
                    return typeof(DateTimeOffset);
                case "datetimeoffset?":
                    return typeof(DateTimeOffset?);
                case "decimal":
                    return typeof(decimal);
                case "decimal?":
                    return typeof(decimal?);
                case "double":
                    return typeof(double);
                case "double?":
                    return typeof(double?);
                case "float":
                    return typeof(float);
                case "float?":
                    return typeof(float?);
                case "guid":
                    return typeof(Guid);
                case "guid?":
                    return typeof(Guid?);
                case "short":
                    return typeof(short);
                case "short?":
                    return typeof(short?);
                case "int":
                    return typeof(int);
                case "int?":
                    return typeof(int?);
                case "long":
                    return typeof(long);
                case "long?":
                    return typeof(long?);
                case "string":
                    return typeof(string);
                case "timespan":
                    return typeof(TimeSpan);
                case "timespan?":
                    return typeof(TimeSpan?);
                default:
                    throw new ArgumentException("typeName");
            }
        }
    }
}
