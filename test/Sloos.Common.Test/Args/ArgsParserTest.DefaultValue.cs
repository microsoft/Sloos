// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        private sealed class MyDefaultOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(DefaultValue = "true")] public bool Bool { get; set; }
            [Option(DefaultValue = "1")] public byte Byte { get; set; }
            [Option(DefaultValue = "2")] public sbyte SByte { get; set; }
            [Option(DefaultValue = "c")] public char Char { get; set; }
            [Option(DefaultValue = "3")] public short Short { get; set; }
            [Option(DefaultValue = "4")] public ushort UShort { get; set; }
            [Option(DefaultValue = "5")] public int Int { get; set; }
            [Option(DefaultValue = "6")] public uint UInt{ get; set; }
            [Option(DefaultValue = "7")] public long Long{ get; set; }
            [Option(DefaultValue = "8")] public ulong ULong{ get; set; }
            [Option(DefaultValue = "1.01")] public float Float { get; set; }
            [Option(DefaultValue = "2.02")] public double Double { get; set; }
            [Option(DefaultValue = "3.03")] public decimal Decimal { get; set; }
            [Option(DefaultValue = "d0b757bf-9441-40c6-b217-484149d52290")] public Guid Guid { get; set; }
            [Option(DefaultValue = "00:01:00")] public TimeSpan TimeSpan { get; set; }
            [Option(DefaultValue = "2000-01-01")] public DateTime DateTime { get; set; }
            [Option(DefaultValue = "2000-02-01")] public DateTimeOffset DateTimeOffset { get; set; }
            [Option(DefaultValue = "string")] public string String { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class MyDefaultOption2
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(DefaultValue = "true")] public bool? Bool { get; set; }
            [Option(DefaultValue = "1")] public byte? Byte { get; set; }
            [Option(DefaultValue = "2")] public sbyte? SByte { get; set; }
            [Option(DefaultValue = "c")] public char? Char { get; set; }
            [Option(DefaultValue = "3")] public short? Short { get; set; }
            [Option(DefaultValue = "4")] public ushort? UShort { get; set; }
            [Option(DefaultValue = "5")] public int? Int { get; set; }
            [Option(DefaultValue = "6")] public uint? UInt{ get; set; }
            [Option(DefaultValue = "7")] public long? Long{ get; set; }
            [Option(DefaultValue = "8")] public ulong? ULong{ get; set; }
            [Option(DefaultValue = "1.01")] public float? Float { get; set; }
            [Option(DefaultValue = "2.02")] public double? Double { get; set; }
            [Option(DefaultValue = "3.03")] public decimal? Decimal { get; set; }
            [Option(DefaultValue = "d0b757bf-9441-40c6-b217-484149d52290")] public Guid? Guid { get; set; }
            [Option(DefaultValue = "00:01:00")] public TimeSpan? TimeSpan { get; set; }
            [Option(DefaultValue = "2000-01-01")] public DateTime? DateTime { get; set; }
            [Option(DefaultValue = "2000-02-01")] public DateTimeOffset? DateTimeOffset { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        [Fact]
        public void ArgsParser_DefaultValue_IsSet()
        {
            var opts1 = ArgsParser<MyDefaultOption1>.Parse(new string[] { });
            Assert.True(opts1.Bool);
            Assert.Equal(1, opts1.Byte);
            Assert.Equal(2, opts1.SByte);
            Assert.Equal('c', opts1.Char);
            Assert.Equal(3, opts1.Short);
            Assert.Equal(4, opts1.UShort);
            Assert.Equal(5, opts1.Int);
            Assert.Equal(6U, opts1.UInt);
            Assert.Equal(7, opts1.Long);
            Assert.Equal(8UL, opts1.ULong);
            Assert.Equal(1.01f, opts1.Float);
            Assert.Equal(2.02, opts1.Double);
            Assert.Equal(3.03m, opts1.Decimal);
            Assert.Equal(Guid.Parse("d0b757bf-9441-40c6-b217-484149d52290"), opts1.Guid);
            Assert.Equal(TimeSpan.FromMinutes(1), opts1.TimeSpan);
            Assert.Equal(DateTime.Parse("2000-01-01"), opts1.DateTime);
            Assert.Equal(DateTimeOffset.Parse("2000-02-01"), opts1.DateTimeOffset);
            Assert.Equal("string", opts1.String);
        }

        [Fact]
        public void ArgsParser_DefaultValueOfNulllable_IsSet()
        {
            var opts1 = ArgsParser<MyDefaultOption2>.Parse(new string[] { });
            // ReSharper disable PossibleInvalidOperationException
            Assert.True(opts1.Bool.Value);
            Assert.Equal(1, opts1.Byte.Value);
            Assert.Equal(2, opts1.SByte.Value);
            Assert.Equal('c', opts1.Char.Value);
            Assert.Equal(3, opts1.Short.Value);
            Assert.Equal(4, opts1.UShort.Value);
            Assert.Equal(5, opts1.Int.Value);
            Assert.Equal(6U, opts1.UInt.Value);
            Assert.Equal(7, opts1.Long.Value);
            Assert.Equal(8UL, opts1.ULong.Value);
            Assert.Equal(1.01f, opts1.Float.Value);
            Assert.Equal(2.02, opts1.Double.Value);
            Assert.Equal(3.03m, opts1.Decimal.Value);
            Assert.Equal(Guid.Parse("d0b757bf-9441-40c6-b217-484149d52290"), opts1.Guid.Value);
            Assert.Equal(TimeSpan.FromMinutes(1), opts1.TimeSpan.Value);
            Assert.Equal(DateTime.Parse("2000-01-01"), opts1.DateTime.Value);
            Assert.Equal(DateTimeOffset.Parse("2000-02-01"), opts1.DateTimeOffset.Value);
            // ReSharper restore PossibleInvalidOperationException
        }
    }
}
