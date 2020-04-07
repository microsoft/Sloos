// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sloos
{
    public static class Extension
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) 
        {
            return source.SelectMany(x => x);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            return source.Concat(new[] { item });
        }

        public static Stream ToStream(this string source)
        {
            return new MemoryStream(
                Encoding.UTF8.GetBytes(source));
        }

        public static Stream ToStream(this StringBuilder source)
        {
            return source.ToString().ToStream();
        }

        public static string FriendlyName(this Type type)
        {
            if (type == typeof(byte))
            {
                return "byte";
            }
            if (type == typeof(bool))
            {
                return "bool";
            }
            if (type == typeof(sbyte))
            {
                return "sbyte";
            }
            if (type == typeof(char))
            {
                return "char";
            }
            if (type == typeof(short))
            {
                return "short";
            }
            if (type == typeof(ushort))
            {
                return "ushort";
            }
            if (type == typeof(int))
            {
                return "int";
            }
            if (type == typeof(uint))
            {
                return "uint";
            }
            if (type == typeof(long))
            {
                return "long";
            }
            if (type == typeof(ulong))
            {
                return "ulong";
            }
            if (type == typeof(float))
            {
                return "float";
            }
            if (type == typeof(double))
            {
                return "double";
            }
            if (type == typeof(decimal))
            {
                return "decimal";
            }
            if (type == typeof(Guid))
            {
                return "Guid";
            }
            if (type == typeof(TimeSpan))
            {
                return "TimeSpan";
            }
            if (type == typeof(DateTime))
            {
                return "DateTime";
            }
            if (type == typeof(DateTimeOffset))
            {
                return "DateTimeOffset";
            }
            if (type == typeof(byte?))
            {
                return "byte?";
            }
            if (type == typeof(bool?))
            {
                return "bool?";
            }
            if (type == typeof(sbyte?))
            {
                return "sbyte?";
            }
            if (type == typeof(char?))
            {
                return "char?";
            }
            if (type == typeof(short?))
            {
                return "short?";
            }
            if (type == typeof(ushort?))
            {
                return "ushort?";
            }
            if (type == typeof(int?))
            {
                return "int?";
            }
            if (type == typeof(uint?))
            {
                return "uint?";
            }
            if (type == typeof(long?))
            {
                return "long?";
            }
            if (type == typeof(ulong?))
            {
                return "ulong?";
            }
            if (type == typeof(float?))
            {
                return "float?";
            }
            if (type == typeof(double?))
            {
                return "double?";
            }
            if (type == typeof(decimal?))
            {
                return "decimal?";
            }
            if (type == typeof(Guid?))
            {
                return "Guid?";
            }
            if (type == typeof(TimeSpan?))
            {
                return "TimeSpan?";
            }
            if (type == typeof(DateTime?))
            {
                return "DateTime?";
            }
            if (type == typeof(DateTimeOffset?))
            {
                return "DateTimeOffset?";
            }

            return type.Name;
        }
    }
}
