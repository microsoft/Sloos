// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        [Fact]
        public void ArgsParser_ParseFlag_IsCorrect()
        {
            var opts1 = ArgsParser<MyLongOption1>.Parse(new[] { "--flag", });
            Assert.True(opts1.Flag);
        }

        [Fact]
        public void ArgsParser_ParseFlagCaseInsensitive_Throws()
        {
            Action test = () => ArgsParser<MyLongOption1>.Parse(new[] { "--FlaG" });
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ArgsParser_MisspelledFlag_Throws()
        {
            Action test = () => ArgsParser<MyLongOption>.Parse(new[] { "--does-not-exist", "--my-int", "0", "--flag" });
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ArgsParser_WithArgValue_IsCorrect()
        {
            var opts = ArgsParser<MyLongOption2>.Parse(new[] { "--my-int", "1" });
            Assert.Equal(1, opts.MyInt);
        }

        [Fact]
        public void ArgsParser_WithArgValueIncorrectType_Throws()
        {
            Action test = () => ArgsParser<MyLongOption2>.Parse(new[] { "--my-int", "abc" });
            test.ShouldThrow<FormatException>();
        }

        [Fact]
        public void ArgsParser_WithArgMissingArg_Throws()
        {
            Action test = () => ArgsParser<MyLongOption2>.Parse(new[] { "--my-int" });
            test.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ArgsParser_WithArgOptionalArgSet_IsCorrect()
        {
            var opts = ArgsParser<MyLongOption>.Parse(new[] { "--flag", "--my-int", "0", "--my-nullable-int", "1" });
            Assert.True(opts.MyNullableInt.HasValue);
            Assert.Equal(1, opts.MyNullableInt.Value);
        }

        [Fact]
        public void ArgsParser_RequiredArgsNotSpecified_Throws()
        {
            Action test = () => ArgsParser<MyLongOption1>.Parse(new string[] { });
            test.ShouldThrow<ArgumentException>();
        }

        private sealed class MyLongOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable UnusedMember.Local
            [Option] public bool Flag { get; set; }
            [Option] public int MyInt { get; set; }
            [Option] public int? MyNullableInt { get; set; }
            // ReSharper restore UnusedMember.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class MyLongOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option] public bool Flag { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class MyLongOption2
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option] public int MyInt { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
