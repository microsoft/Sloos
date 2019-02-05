// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public sealed class CsvSerializerHeaderFactoryTest
    {
        [Fact]
        public void CsvSerializerHeaderFactory_MixedValuePropertyTypes()
        {
            var factory = new CsvSerializerHeaderFactory(typeof(TestData.MixedValuePropertyTypes));
            var columns = factory.GetDelimitedColumns()
                .ToArray();

            columns[0].Name.Should().Be("Field01");
            columns[1].Name.Should().Be("Field02");
            columns[2].Name.Should().Be("Field03");
            columns[3].Name.Should().Be("Field04");
            columns[4].Name.Should().Be("Field05");
            columns[5].Name.Should().Be("Field06");
            columns[6].Name.Should().Be("Field07");
            columns[7].Name.Should().Be("Field08");
            columns[8].Name.Should().Be("Field09");
            columns[9].Name.Should().Be("Field10");
            columns[10].Name.Should().Be("Field11");
            columns[11].Name.Should().Be("Field12");
            columns[12].Name.Should().Be("Field13");
            columns[13].Name.Should().Be("Field14");
            columns[14].Name.Should().Be("Field15");
            columns[15].Name.Should().Be("Field16");

            columns[0].Type.Should().Be(typeof(byte));
            columns[1].Type.Should().Be(typeof(sbyte));
            columns[2].Type.Should().Be(typeof(char));
            columns[3].Type.Should().Be(typeof(short));
            columns[4].Type.Should().Be(typeof(ushort));
            columns[5].Type.Should().Be(typeof(int));
            columns[6].Type.Should().Be(typeof(uint));
            columns[7].Type.Should().Be(typeof(long));
            columns[8].Type.Should().Be(typeof(ulong));
            columns[9].Type.Should().Be(typeof(float));
            columns[10].Type.Should().Be(typeof(double));
            columns[11].Type.Should().Be(typeof(decimal));
            columns[12].Type.Should().Be(typeof(Guid));
            columns[13].Type.Should().Be(typeof(TimeSpan));
            columns[14].Type.Should().Be(typeof(DateTime));
            columns[15].Type.Should().Be(typeof(DateTimeOffset));
        }

        [Fact]
        public void CsvSerializerHeaderFactory_MixedNullablePropertyTypes()
        {
            var factory = new CsvSerializerHeaderFactory(typeof(TestData.MixedNullablePropertyTypes));
            var columns = factory.GetDelimitedColumns()
                .ToArray();

            columns[0].Type.Should().Be(typeof(byte?));
            columns[1].Type.Should().Be(typeof(sbyte?));
            columns[2].Type.Should().Be(typeof(char?));
            columns[3].Type.Should().Be(typeof(short?));
            columns[4].Type.Should().Be(typeof(ushort?));
            columns[5].Type.Should().Be(typeof(int?));
            columns[6].Type.Should().Be(typeof(uint?));
            columns[7].Type.Should().Be(typeof(long?));
            columns[8].Type.Should().Be(typeof(ulong?));
            columns[9].Type.Should().Be(typeof(float?));
            columns[10].Type.Should().Be(typeof(double?));
            columns[11].Type.Should().Be(typeof(decimal?));
            columns[12].Type.Should().Be(typeof(Guid?));
            columns[13].Type.Should().Be(typeof(TimeSpan?));
            columns[14].Type.Should().Be(typeof(DateTime?));
            columns[15].Type.Should().Be(typeof(DateTimeOffset?));
        }

        [Fact]
        public void CsvSerializerHeaderFactory_StringType()
        {
            var factory = new CsvSerializerHeaderFactory(typeof(TestData.Simple));
            var columns = factory.GetDelimitedColumns()
                .ToArray();

            columns[0].Name.Should().Be("Field");
            columns[0].Type.Should().Be(typeof(string));
        }
    }
}
