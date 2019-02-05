// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using Xunit;

namespace Sloos.Common.Test.Args
{
    public class ParsedArgFactoryTest
    {
        [Fact]
        public void ParsedArgFactory_Parse_OnlyOptionAttributes()
        {
            var testSubject = ParsedArgFactory.Create<TestOption>();
            Assert.Equal(5, testSubject.Count);

            Assert.True(testSubject.ContainsKey("my-bool"));
            Assert.True(testSubject.ContainsKey("my-int"));
            Assert.True(testSubject.ContainsKey("my-nullable-int"));
            Assert.True(testSubject.ContainsKey("my-string1"));
            Assert.True(testSubject.ContainsKey("my-string2"));
        }

        [Fact]
        public void ParsedArgFactory_Lookup_IsNotCaseInsensitive()
        {
            var testSubject = ParsedArgFactory.Create<TestOption>();

            Assert.True(testSubject.ContainsKey("my-bool"));
            Assert.False(testSubject.ContainsKey("MyBool"));
        }

        [Fact]
        public void ParsedArgFactory_LookupWithOverriddenName_IsNotCaseInsensitive()
        {
            var factory = ParsedArgFactory.Create<TestOption1>();

            Assert.False(factory.ContainsKey("MyBool"));

            Assert.True(factory.ContainsKey("donkey"));
            Assert.True(factory.ContainsKey("d"));

            Assert.False(factory.ContainsKey("DonkeY"));
            Assert.False(factory.ContainsKey("D"));
        }

        [Fact]
        public void ParsedArgFactory_LongNamePropertyAcrossParsedArgs_IsIdentical()
        {
            var factory = ParsedArgFactory.Create<TestOption1>();

            var parsedArg1 = factory["donkey"];
            var parsedArg2 = factory["d"];

            Assert.Equal("donkey", parsedArg1.LongName);
            Assert.Equal(parsedArg1.LongName, parsedArg2.LongName);
        }

        [Fact]
        public void ParsedArgFactory_WithShortName_IsCorrect()
        {
            var factory = ParsedArgFactory.Create<TestOption2>();

            Assert.True(factory.ContainsKey("my-bool"));
            Assert.True(factory.ContainsKey("d"));
        }

        [Fact]
        public void ParsedArgFactory_UnparsedArguments_PropertyInfoNotNull()
        {
            var factory = new ParsedArgFactory(typeof(UnparsedArgumentsOption1));

            Assert.NotNull(factory.UnparsedArgs);
            Assert.Equal("Args", factory.UnparsedArgs.Name);
        }

        [Fact]
        public void ParsedArgFactory_NoUnparsedArguments_PropertyInfoIsNull()
        {
            var factory = new ParsedArgFactory(typeof(TestOption));
            Assert.Null(factory.UnparsedArgs);
        }

        [Fact]
        public void ParsedArgFactory_TooManyUnparsedArguments_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action test = () => new ParsedArgFactory(typeof(UnparsedArgumentsOption2));
            test.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ParsedArgFactory_NonStringIEnumerable_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action test = () => new ParsedArgFactory(typeof(UnparsedArgumentsOption3));
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ParsedArgFactory_NonIEnumerable_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action test = () => new ParsedArgFactory(typeof(UnparsedArgumentsOption4));
            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ParsedArgFactory_Derived_Success()
        {
            var factory = ParsedArgFactory.Create<ChildOption>();

            Assert.True(factory.ContainsKey("flag"));
            Assert.True(factory.ContainsKey("help"));
        }

        private sealed class ChildOption : ParentOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option] public bool Help { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private class ParentOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option] public bool Flag { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class TestOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable UnusedMember.Local
            [Option] public bool MyBool { get; set; }
            [Option] public int MyInt { get; set; }
            [Option] public int? MyNullableInt { get; set; }
            [Option] public string MyString1 { get; set; }
            [Option] public string MyString2 { get; set; }
            public bool IsMyString2 { get; set; }

            bool IsIgnored { get; set; }
            // ReSharper restore UnusedMember.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class TestOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option(LongName = "donkey", ShortName = "d")] public bool MyBool { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class TestOption2
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option(ShortName = "d")] public bool MyBool { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class UnparsedArgumentsOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [UnparsedArguments] public string[] Args { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class UnparsedArgumentsOption2
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable UnusedMember.Local
            [UnparsedArguments] public string[] Args1 { get; set; }
            [UnparsedArguments] public string[] Args2 { get; set; }
            // ReSharper restore once UnusedMember.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class UnparsedArgumentsOption3
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [UnparsedArguments] public int[] Args { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class UnparsedArgumentsOption4
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [UnparsedArguments] public string Args { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
