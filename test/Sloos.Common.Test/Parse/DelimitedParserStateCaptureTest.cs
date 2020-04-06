// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedParserStateCaptureTest
    {
        [Fact]
        public void DelimitedParserStateCapture_InitialState_IsNotComplete()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateCapture>(',');
            Assert.Empty(testSubject.Fields);
        }

        [Fact]
        public void DelimitedParseStateCapture_EmptyField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateCapture>(',');
            testSubject.Parse(",");

            Assert.Single(testSubject.Fields);
            Assert.Equal("", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateCapture_CaptureUpToDelimiter()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateCapture>(',');
            testSubject.Parse("foo,");

            Assert.Single(testSubject.Fields);
            Assert.Equal("foo", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateCapture_Crlf_DoNotCapture()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateCapture>(',');
            testSubject.Parse("foo\r\n");

            Assert.Single(testSubject.Fields);
            Assert.Equal("foo", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateCapture_LF_DoNotCapture()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateCapture>(',');
            testSubject.Parse("foo\n");

            Assert.Single(testSubject.Fields);
            Assert.Equal("foo", testSubject.Fields[0]);
        }
    }
}
