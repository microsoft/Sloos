// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sloos.Common
{
    public sealed class TypeElector
    {
        private readonly TypeInfo[] converters;

        public TypeElector()
        {
            this.converters = new[] {
                // Order is important!
                GuidTypeInfo.Create(1),     // TypeInfo.Create(1, typeof(Guid)),
                IntTypeInfo.Create(2),      // TypeInfo.Create(2, typeof(int)),
                LongTypeInfo.Create(3),     // TypeInfo.Create(3, typeof(long)),
                ULongTypeInfo.Create(4),    // TypeInfo.Create(4, typeof(ulong)),
                DoubleTypeInfo.Create(5),   // TypeInfo.Create(5, typeof(double)),
                CharTypeInfo.Create(6),     // TypeInfo.Create(6, typeof(char)),
                // typeof(byte)
                BoolTypeInfo.Create(7),     // TypeInfo.Create(7, typeof(bool)),
                // typeof(sbyte)
                // typeof(short)
                // typeof(ushort)
                // typeof(uint)
                // typeof(float)
                // typeof(decimal)
                TimeSpanTypeInfo.Create(8), // TypeInfo.Create(8, typeof(TimeSpan)),
                DateTimeTypeInfo.Create(9), // TypeInfo.Create(9, typeof(DateTime)),
                // typeof(DateTimeOffset)
                TypeInfo.Create(10, typeof(string)),
            };
        }

        public global::System.Type Elect(string[] strings)
        {
            bool allNullOrEmpty = strings.All(string.IsNullOrWhiteSpace);
            if (allNullOrEmpty)
            {
                return typeof(string);
            }

            bool isNullable = strings.Any(string.IsNullOrEmpty);
            var mostCommonType = this.FindMostRelevantType(strings);

            return (isNullable && mostCommonType != typeof(string)) ?
                typeof(Nullable<>).MakeGenericType(mostCommonType) :
                mostCommonType;
        }

        private Type FindMostRelevantType(string[] strings)
        {
            var typeInfo = strings
                .Where(x => !string.IsNullOrEmpty(x))
                .SelectMany(this.GetMatchingTypeInfos)
                .GroupBy(x => x)
                .Select(x => new {
                    TypeInfo = x.Key, 
                    Count = x.Count(),
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TypeInfo.Order)
                .First();

            return typeInfo.TypeInfo.Type;
        }

        private TypeInfo[] GetMatchingTypeInfos(string s)
        {
            return this.converters
                .Where(x => x.Maybe(s))
                .Where(x => this.IsConvertible(x.Converter, s))
                .ToArray();
        }

        private bool IsConvertible(TypeConverter converter, string s)
        {
            try
            {
                converter.ConvertFromInvariantString(s);
                return true;
            }
            catch
            {
                // Ignored
            }

            return false;
        }
    }
}
