// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        [Fact]
        public void ArgsParserShort_ParseFlag_IsCorrect()
        {
            var opts1 = ArgsParser<MyShortOption1>.Parse(new[] { "-f", });
            Assert.True(opts1.Flag);
        }

        [Fact]
        public void ArgsParserShort_ParseFlagCaseInsensitive_Throws()
        {
            Action test = () => ArgsParser<MyShortOption1>.Parse(new[] { "-F" });
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ArgsParserShort_MisspelledFlag_Throws()
        {
            Action test = () => ArgsParser<MyShortOption>.Parse(new[] { "--does-not-exist", "-m", "0", "-f" });
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ArgsParserShort_WithArgValue_IsCorrect()
        {
            var opts = ArgsParser<MyShortOption2>.Parse(new[] { "-m", "1" });
            Assert.Equal(1, opts.MyInt);
        }

        [Fact]
        public void ArgsParserShort_WithArgValueIncorrectType_Throws()
        {
            Action test = () => ArgsParser<MyShortOption2>.Parse(new[] { "-m", "abc" });
            test.ShouldThrow<FormatException>();
        }

        [Fact]
        public void ArgsParserShort_WithArgMissingArg_Throws()
        {
            Action test = () => ArgsParser<MyShortOption2>.Parse(new[] { "-m" });
            test.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ArgsParserShort_WithArgOptionalArgSet_IsCorrect()
        {
            var opts = ArgsParser<MyShortOption>.Parse(new[] { "-f", "-m", "0", "-n", "1" });
            Assert.True(opts.MyNullableInt.HasValue);
            Assert.Equal(1, opts.MyNullableInt.Value);
        }

        [Fact]
        public void ArgsParserShort_CombinedArgumentFlags_IsCorrect()
        {
            var opts = ArgsParser<MyShortOption>.Parse(new[] { "-fg", "-m", "1" });

            Assert.True(opts.Flag1);
            Assert.True(opts.Flag2);
            Assert.Equal(1, opts.MyInt);
        }

        [Fact]
        public void ArgsParserShort_CombinedArgumentWithValue_IsCorrect()
        {
            var opts = ArgsParser<MyShortOption>.Parse(new[] { "-fgm", "1" });

            Assert.True(opts.Flag1);
            Assert.True(opts.Flag2);
            Assert.Equal(1, opts.MyInt);
        }

        [Fact]
        public void ArgsParserShort_CombinedArgumentWithValueWithEqual_IsCorrect()
        {
            Action test = () => ArgsParser<MyShortOption>.Parse(new[] { "-fgm=1" });
            test.ShouldThrow<ArgumentException>();
        }

        private sealed class MyShortOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(ShortName="f")] public bool Flag1 { get; set; }
            [Option(ShortName="g", DefaultValue="false")] public bool Flag2 { get; set; }
            [Option(ShortName="m")] public int MyInt { get; set; }
            [Option(ShortName="n")] public int? MyNullableInt { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class MyShortOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(ShortName = "f")] public bool Flag { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class MyShortOption2
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(ShortName="m")] public int MyInt { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
