// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Slam.Common.Test.Args
{
    //public class MyStringConstructable
    //{
    //    public MyStringConstructable(string arg)
    //    {
    //        this.Arg = arg;
    //    }

    //    public string Arg { get; private set; }
    //}

    public class MyOption
    {
        [Option] public bool Flag { get; set; }
        [Option] public int MyInt { get; set; }
        [Option] public int? MyNullableInt { get; set; }
    }

    public class MyOption1
    {
        [Option] public bool Flag { get; set; }
    }

    public class MyOption2
    {
        [Option] public int MyInt { get; set; }
    }


    //public class MyOption
    //{
    //    public int Value { get; set; }
    //    public int? Optional { get; set; }
    //    public bool Flag { get; set; }

    //    public IEnumerable<int> Integers { get; set; } 

    //    public string MyString { get; set; }
    //    public bool HasMyString { get; set; }

    //    public MyStringConstructable MyStringConstructable { get; set; }
    //}

    public class ArgsParserTest
    {
        [Fact]
        public void ArgsParser_ParseFlag_IsCorrect()
        {
            var opts1 = ArgsParser<MyOption1>.Parse(new[] { "--flag", });
            Assert.True(opts1.Flag);
        }

        [Fact]
        public void ArgsParser_ParseFlagCaseInsensitive_Throws()
        {
            Action test = () => ArgsParser<MyOption1>.Parse(new[] { "--FlaG" });
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ArgsParser_MisspelledFlag_Throws()
        {
            Action test = () => ArgsParser<MyOption>.Parse(new[] { "--does-not-exist", "--my-int", "0", "--flag" });
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ArgsParser_WithArgValue_IsCorrect()
        {
            var opts = ArgsParser<MyOption2>.Parse(new[] { "--my-int", "1" });
            Assert.Equal(1, opts.MyInt);
        }

        [Fact]
        public void ArgsParser_WithArgValueIncorrectType_Throws()
        {
            Action test = () => ArgsParser<MyOption2>.Parse(new[] { "--my-int", "abc" });
            test.ShouldThrow<FormatException>();
        }

        [Fact]
        public void ArgsParser_WithArgMissingArg_Throws()
        {
            Action test = () => ArgsParser<MyOption2>.Parse(new[] { "--my-int" });
            test.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ArgsParser_WithArgOptionalArgSet_IsCorrect()
        {
            var opts = ArgsParser<MyOption>.Parse(new[] { "--flag", "--my-int", "0", "--my-nullable-int", "1" });
            Assert.True(opts.MyNullableInt.HasValue);
            Assert.Equal(1, opts.MyNullableInt.Value);
        }

        [Fact]
        public void ArgsParser_RequiredArgsNotSpecified_Throws()
        {
            Action test = () => ArgsParser<MyOption1>.Parse(new string[] { });
            test.ShouldThrow<ArgumentException>();
        }
    }
}
