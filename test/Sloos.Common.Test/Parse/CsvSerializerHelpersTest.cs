// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using Xunit;

namespace Sloos.Common.Test
{
    public class CsvSerializerHelpersTest
    {
        [Fact]
        public void CsvSerializerHelpers_ImplicitlyOrdered()
        {
            var testSubject = CsvSerializerHelpers.OrderByProperty(typeof(TestData.ImplicitlyOrdered))
                .ToArray();

            Assert.Equal(3, testSubject.Length);
            Assert.Equal("Field1", testSubject[0].Name);
            Assert.Equal("Field2", testSubject[1].Name);
            Assert.Equal("Field3", testSubject[2].Name);
        }

        [Fact]
        public void CsvSerializerHelpers_NamedMembers()
        {
            // TODO: support named members?
            var testSubject = CsvSerializerHelpers.OrderByProperty(typeof(TestData.NamedMembers))
                .ToArray();

            Assert.Equal(1, testSubject.Length);
            Assert.Equal("Field", testSubject[0].Name);
        }

        [Fact]
        public void CsvSerializerHelpers_ExplicitlyOrdered()
        {
            var testSubject = CsvSerializerHelpers.OrderByProperty(typeof(TestData.ExplicitlyOrdered))
                .ToArray();

            Assert.Equal(4, testSubject.Length);
            Assert.Equal("Field0", testSubject[0].Name);
            Assert.Equal("Field1", testSubject[1].Name);
            Assert.Equal("Field2", testSubject[2].Name);
            Assert.Equal("Field3", testSubject[3].Name);
        }

        [Fact]
        public void CsvSerializerHelpers_OrderByOrderThenByName()
        {
            var testSubject = CsvSerializerHelpers.OrderByProperty(typeof(TestData.OrderByOrderThenByName))
                .ToArray();

            Assert.Equal(3, testSubject.Length);
            Assert.Equal("Field1", testSubject[0].Name);
            Assert.Equal("Field2", testSubject[1].Name);
            Assert.Equal("Field3", testSubject[2].Name);
        }

        [Fact]
        public void CsvSerializerHelpers_ComplexOrderByThenBy()
        {
            var testSubject = CsvSerializerHelpers.OrderByProperty(typeof(TestData.ComplexOrderByThenBy))
                .ToArray();

            Assert.Equal("aac", testSubject[0].Name);
            Assert.Equal("Aac", testSubject[1].Name);
            Assert.Equal("Aad", testSubject[2].Name);
            Assert.Equal("Aab", testSubject[3].Name);
            Assert.Equal("Aaa", testSubject[4].Name);
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_DoNotTrimStringValues()
        {
            const string s = "  field0  ";
            Assert.Equal(s, CoercionHelpers.CreateCoercionMap()[typeof(string)](s));
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_ValueType()
        {
            var guid1 = Guid.Parse("b5cb729f-cb55-45a4-a4b8-5e0dc218ee04");
            var dateTime1 = DateTime.Parse("2000-01-01");

            Assert.Equal(15, this.CoerceStringToType<byte>("15"));
            Assert.Equal(16, this.CoerceStringToType<sbyte>("16"));
            Assert.Equal('f', this.CoerceStringToType<char>("f"));
            Assert.Equal(1234, this.CoerceStringToType<short>("1234"));
            Assert.Equal(1234, this.CoerceStringToType<ushort>("1234"));
            Assert.Equal(12341234, this.CoerceStringToType<int>("12341234"));
            Assert.Equal((uint)12341234, this.CoerceStringToType<uint>("12341234"));
            Assert.Equal(1234123412341234, this.CoerceStringToType<long>("1234123412341234"));
            Assert.Equal((ulong)1234123412341234, this.CoerceStringToType<ulong>("1234123412341234"));
            Assert.Equal(1.01f, this.CoerceStringToType<float>("1.01"));
            Assert.Equal(2.02, this.CoerceStringToType<double>("2.02"));
            Assert.Equal(3.03m, this.CoerceStringToType<decimal>("3.03"));
            Assert.Equal(guid1, this.CoerceStringToType<Guid>("b5cb729f-cb55-45a4-a4b8-5e0dc218ee04"));
            Assert.Equal(TimeSpan.FromMinutes(1), this.CoerceStringToType<TimeSpan>("00:01:00"));
            Assert.Equal(dateTime1, this.CoerceStringToType<DateTime>("2000-01-01"));
            Assert.Equal(DateTimeOffset.Parse("2000-01-01T00:00:00+00:00"), this.CoerceStringToType<DateTimeOffset>("2000-01-01T00:00:00+00:00"));
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_DateTime()
        {
            Assert.Equal(DateTimeKind.Unspecified, this.CoerceStringToType<DateTime>("2000-01-01").Kind);
            Assert.Equal(DateTimeKind.Utc, this.CoerceStringToType<DateTime>("2000-01-01T00:00:00Z").Kind);
            Assert.Equal(DateTimeKind.Utc, this.CoerceStringToType<DateTime>("2000-01-01T00:00:00-04:00").Kind);
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_NullableDateTime()
        {
            // ReSharper disable PossibleInvalidOperationException
            Assert.Equal(DateTimeKind.Unspecified, this.CoerceStringToType<DateTime?>("2000-01-01").Value.Kind);
            Assert.Equal(DateTimeKind.Utc, this.CoerceStringToType<DateTime?>("2000-01-01T00:00:00Z").Value.Kind);
            Assert.Equal(DateTimeKind.Utc, this.CoerceStringToType<DateTime?>("2000-01-01T00:00:00-04:00").Value.Kind);
            // ReSharper restore PossibleInvalidOperationException
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_NullableType()
        {
            var guid1 = Guid.Parse("b5cb729f-cb55-45a4-a4b8-5e0dc218ee04");
            var dateTime1 = DateTime.Parse("2000-01-01");

            // ReSharper disable PossibleInvalidOperationException
            Assert.Equal(15, this.CoerceStringToType<byte?>("15  ").Value);
            Assert.Equal(16, this.CoerceStringToType<sbyte?>("16  ").Value);
            Assert.Equal('f', this.CoerceStringToType<char?>("f  ").Value);
            Assert.Equal(1234, this.CoerceStringToType<short?>("1234  ").Value);
            Assert.Equal(1234, this.CoerceStringToType<ushort?>("1234  ").Value);
            Assert.Equal(12341234, this.CoerceStringToType<int?>("12341234  ").Value);
            Assert.Equal((uint)12341234, this.CoerceStringToType<uint?>("12341234  ").Value);
            Assert.Equal(1234123412341234, this.CoerceStringToType<long?>("1234123412341234  ").Value);
            Assert.Equal((ulong)1234123412341234, this.CoerceStringToType<ulong?>("1234123412341234  ").Value);
            Assert.Equal(1.01f, this.CoerceStringToType<float?>("1.01  ").Value);
            Assert.Equal(2.02, this.CoerceStringToType<double?>("2.02  ").Value);
            Assert.Equal(3.03m, this.CoerceStringToType<decimal?>("3.03  ").Value);
            Assert.Equal(guid1, this.CoerceStringToType<Guid?>("b5cb729f-cb55-45a4-a4b8-5e0dc218ee04").Value);
            Assert.Equal(TimeSpan.FromMinutes(1), this.CoerceStringToType<TimeSpan?>("00:01:00  ").Value);
            Assert.Equal(dateTime1, this.CoerceStringToType<DateTime?>("2000-01-01  ").Value);
            Assert.Equal(DateTimeOffset.Parse("2000-01-01T00:00:00+00:00"), this.CoerceStringToType<DateTimeOffset?>("2000-01-01T00:00:00+00:00  ").Value);
            // ReSharper restore PossibleInvalidOperationException
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_TrimWhitespace()
        {
            var guid1 = Guid.Parse("b5cb729f-cb55-45a4-a4b8-5e0dc218ee04");
            var dateTime1 = DateTime.Parse("2000-01-01");

            // ReSharper disable PossibleInvalidOperationException
            Assert.Equal(15, this.CoerceStringToType<byte>("  15  "));
            Assert.Equal(16, this.CoerceStringToType<sbyte>("  16  "));
            Assert.Equal('f', this.CoerceStringToType<char>("  f  "));
            Assert.Equal(1234, this.CoerceStringToType<short>("  1234  "));
            Assert.Equal(1234, this.CoerceStringToType<ushort>("  1234  "));
            Assert.Equal(12341234, this.CoerceStringToType<int>("  12341234  "));
            Assert.Equal((uint)12341234, this.CoerceStringToType<uint>("  12341234  "));
            Assert.Equal(1234123412341234, this.CoerceStringToType<long>("  1234123412341234  "));
            Assert.Equal((ulong)1234123412341234, this.CoerceStringToType<ulong>("  1234123412341234  "));
            Assert.Equal(1.01f, this.CoerceStringToType<float>("  1.01  "));
            Assert.Equal(2.02, this.CoerceStringToType<double>("  2.02  "));
            Assert.Equal(3.03m, this.CoerceStringToType<decimal>("  3.03  "));
            Assert.Equal(guid1, this.CoerceStringToType<Guid>("  b5cb729f-cb55-45a4-a4b8-5e0dc218ee04  "));
            Assert.Equal(TimeSpan.FromMinutes(1), this.CoerceStringToType<TimeSpan>("  00:01:00  "));
            Assert.Equal(dateTime1, this.CoerceStringToType<DateTime>("  2000-01-01  "));
            Assert.Equal(DateTimeOffset.Parse("2000-01-01T00:00:00+00:00"), this.CoerceStringToType<DateTimeOffset>("  2000-01-01T00:00:00+00:00  "));

            Assert.Equal(15, this.CoerceStringToType<byte?>("  15  ").Value);
            Assert.Equal(16, this.CoerceStringToType<sbyte?>("  16  ").Value);
            Assert.Equal('f', this.CoerceStringToType<char?>("  f  ").Value);
            Assert.Equal(1234, this.CoerceStringToType<short?>("  1234  ").Value);
            Assert.Equal(1234, this.CoerceStringToType<ushort?>("  1234  ").Value);
            Assert.Equal(12341234, this.CoerceStringToType<int?>("  12341234  ").Value);
            Assert.Equal((uint)12341234, this.CoerceStringToType<uint?>("  12341234  ").Value);
            Assert.Equal(1234123412341234, this.CoerceStringToType<long?>("  1234123412341234  ").Value);
            Assert.Equal((ulong)1234123412341234, this.CoerceStringToType<ulong?>("  1234123412341234  ").Value);
            Assert.Equal(1.01f, this.CoerceStringToType<float?>("  1.01  ").Value);
            Assert.Equal(2.02, this.CoerceStringToType<double?>("  2.02  ").Value);
            Assert.Equal(3.03m, this.CoerceStringToType<decimal?>("  3.03  ").Value);
            Assert.Equal(guid1, this.CoerceStringToType<Guid?>("  b5cb729f-cb55-45a4-a4b8-5e0dc218ee04  ").Value);
            Assert.Equal(TimeSpan.FromMinutes(1), this.CoerceStringToType<TimeSpan?>("  00:01:00  ").Value);
            Assert.Equal(dateTime1, this.CoerceStringToType<DateTime?>("  2000-01-01  ").Value);
            Assert.Equal(DateTimeOffset.Parse("2000-01-01T00:00:00+00:00"), this.CoerceStringToType<DateTimeOffset?>("  2000-01-01T00:00:00+00:00  ").Value);
            // ReSharper restore PossibleInvalidOperationException
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_NullableType_StringEmpty()
        {
            Assert.False(this.CoerceStringToType<byte?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<sbyte?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<char?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<short?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<ushort?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<int?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<uint?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<long?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<ulong?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<float?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<double?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<decimal?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<Guid?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<TimeSpan?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<DateTime?>(string.Empty).HasValue);
            Assert.False(this.CoerceStringToType<DateTimeOffset?>(string.Empty).HasValue);
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_NullableType_StringWhitespace()
        {
            Assert.False(this.CoerceStringToType<byte?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<sbyte?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<char?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<short?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<ushort?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<int?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<uint?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<long?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<ulong?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<float?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<double?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<decimal?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<Guid?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<TimeSpan?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<DateTime?>("  ").HasValue);
            Assert.False(this.CoerceStringToType<DateTimeOffset?>("  ").HasValue);
        }

        [Fact]
        public void CsvSerializerHelpers_CoercionMap_NullableType_Null()
        {
            Assert.False(this.CoerceStringToType<byte?>(null).HasValue);
            Assert.False(this.CoerceStringToType<sbyte?>(null).HasValue);
            Assert.False(this.CoerceStringToType<char?>(null).HasValue);
            Assert.False(this.CoerceStringToType<short?>(null).HasValue);
            Assert.False(this.CoerceStringToType<ushort?>(null).HasValue);
            Assert.False(this.CoerceStringToType<int?>(null).HasValue);
            Assert.False(this.CoerceStringToType<uint?>(null).HasValue);
            Assert.False(this.CoerceStringToType<long?>(null).HasValue);
            Assert.False(this.CoerceStringToType<ulong?>(null).HasValue);
            Assert.False(this.CoerceStringToType<float?>(null).HasValue);
            Assert.False(this.CoerceStringToType<double?>(null).HasValue);
            Assert.False(this.CoerceStringToType<decimal?>(null).HasValue);
            Assert.False(this.CoerceStringToType<Guid?>(null).HasValue);
            Assert.False(this.CoerceStringToType<TimeSpan?>(null).HasValue);
            Assert.False(this.CoerceStringToType<DateTime?>(null).HasValue);
            Assert.False(this.CoerceStringToType<DateTimeOffset?>(null).HasValue);
        }

        private T CoerceStringToType<T>(string s)
        {
            return (T)CoercionHelpers.CreateCoercionMap()[typeof(T)](s);
        }
    }
}
