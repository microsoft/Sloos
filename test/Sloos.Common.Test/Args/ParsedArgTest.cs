// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test.Args
{
    public class ParsedArgTest
    {
        [Fact]
        public void ParsedArg_BareOptionAttribute_ReasonableDefaults()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("Flag1"));

            Assert.True(arg.IsRequired);
            Assert.False(arg.HasValue);
            Assert.True(string.IsNullOrEmpty(arg.DefaultValue));
            Assert.Equal("flag1", arg.LongName);
            Assert.True(string.IsNullOrEmpty(arg.ShortName));
        }

        [Fact]
        public void ParsedArg_LongName_Overrides()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("Flag2"));

            Assert.False(arg.IsRequired);
            Assert.False(arg.HasValue);
            Assert.Equal("true", arg.DefaultValue);
            Assert.Equal("MyFlag", arg.LongName);
            Assert.Equal("f", arg.ShortName);
        }

        [Fact]
        public void ParsedArg_OptionWithValue_HasValue()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("Int1"));

            Assert.True(arg.IsRequired);
            Assert.True(arg.HasValue);
            Assert.Equal("int1", arg.LongName);
        }

        [Fact]
        public void ParsedArg_NullableWithValue_IsNotRequired()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("Int2"));

            Assert.False(arg.IsRequired);
            Assert.True(arg.HasValue);
            Assert.Equal("int2", arg.LongName);
        }

        [Fact]
        public void ParsedArg_StringMethodNoIsMethod_IsRequired()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("String1"));

            Assert.True(arg.IsRequired);
        }

        [Fact]
        public void ParsedArg_StringIsRequiredMethod_MatchesOnIs()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("String2"));

            Assert.False(arg.IsRequired);
        }

        [Fact]
        public void ParsedArg_StringIsRequiredMethod_IsCaseSensitive()
        {
            var type = typeof(MyOption);
            var arg = ParsedArg.Create(type, type.GetProperty("String3"));

            Assert.True(arg.IsRequired);
        }

        private sealed class MyOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable UnusedMember.Local
            [Option] public bool Flag1 { get; set; }
            [Option(LongName="MyFlag", ShortName="f", DefaultValue="true")] public bool Flag2 { get; set; }

            [Option] public int Int1 { get; set; }
            [Option] public int? Int2 { get; set; }
            
            [Option] public string String1 { get; set; }
            [Option] public string String2 { get; set; }
            public bool IsString2 { get; set; }

            [Option] public string String3 { get; set; }
            // ReSharper disable once InconsistentNaming
            public bool isString3 { get; set; }
            // ReSharper restore UnusedMember.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
