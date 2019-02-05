// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedParserStateDelimitTest
    {
        [Fact]
        public void DelimitedParserStateDelimit_InitialState_IsNotComplete()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateDelimit>(',');
            testSubject.Parse(string.Empty);

            Assert.Empty(testSubject.Fields);
        }

        [Fact]
        public void DelimitedParseStateDelimit_EmptyField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateDelimit>(',');
            testSubject.Parse(",");

            Assert.Empty(testSubject.Fields);
        }

        [Fact]
        public void DelimitedParseStateDelimit_EmptyFields_DataNotAccumulated()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateDelimit>(',');
            testSubject.Parse("abc,");

            Assert.Empty(testSubject.Fields);
        }
    }
}
