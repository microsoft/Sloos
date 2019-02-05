// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public class ArgsHelpersTest
    {
        [Fact]
        public void PropertyToOptionName_Simple_IsCorrect()
        {
            var testSubject = ArgsHelpers.PropertyToOptionName("Name");
            Assert.Equal("name", testSubject);
        }

        [Fact]
        public void PropertyToOptionName_CamelCase_IsCorrect()
        {
            var testSubject = ArgsHelpers.PropertyToOptionName("ThisIsName");
            Assert.Equal("this-is-name", testSubject);
        }

        [Fact]
        public void PropertyToOptionName_PascalCase_IsCorrect()
        {
            var testSubject = ArgsHelpers.PropertyToOptionName("thisIsName");
            Assert.Equal("this-is-name", testSubject);
        }
    }
}
