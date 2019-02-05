// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sloos.Common
{
    public sealed class TypeElector
    {
        private readonly TypeInfo[] converters;

        public TypeElector()
        {
            this.converters = new[] {
                // Order is important!
                TypeInfo.Create(1, typeof(Guid)), 
                TypeInfo.Create(2, typeof(int)),
                TypeInfo.Create(3, typeof(long)),
                TypeInfo.Create(4, typeof(ulong)),
                TypeInfo.Create(5, typeof(double)),
                TypeInfo.Create(6, typeof(char)),
                // typeof(byte)
                TypeInfo.Create(7, typeof(bool)),
                // typeof(sbyte)
                // typeof(short)
                // typeof(ushort)
                // typeof(uint)
                // typeof(float)
                // typeof(decimal)
                TypeInfo.Create(8, typeof(TimeSpan)),
                TypeInfo.Create(9, typeof(DateTime)),
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

        private Type FindMostRelevantType(IEnumerable<string> strings)
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

        private IEnumerable<TypeInfo> GetMatchingTypeInfos(string s)
        {
            return this.converters
                .Where(x => this.IsConvertible(x.Converter, s));
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
