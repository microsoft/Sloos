// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedParserStateQuoteTest
    {
        [Fact]
        public void DelimitedParseStateQuote_InitialState_IsNotComplete()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            Assert.Empty(testSubject.Fields);
        }

        [Fact]
        public void DelimitedParseStateQuote_IsQuoteChar()
        {
            Assert.True(DelimitedParserStateQuote.IsQuote('"'));

            Assert.False(DelimitedParserStateQuote.IsQuote('a'));
        }

        [Fact]
        public void DelimitedParseStateQuote_DoubleQuotes_ParseQuotedField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            testSubject.Parse(("foo\""));

            Assert.Equal(1, testSubject.Fields.Count);
            Assert.Equal("foo", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateQuote_EscapedQuotes_ParseQuotedField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            testSubject.Parse("f\"\"oo\"");

            Assert.Equal(1, testSubject.Fields.Count);
            Assert.Equal("f\"oo", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateQuote_MissingEndQuote_DoesNotComplete()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            testSubject.Parse("f\"\"oo");

            Assert.Empty(testSubject.Fields);
        }

        [Fact]
        public void DelimitedParseStateQuote_EscapedNonQuoteCharacter_ParseQuotedField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            testSubject.Parse("f\\roo\"");

            Assert.Equal(1, testSubject.Fields.Count);
            Assert.Equal("f\\roo", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateQuote_QuoteAndDelimiter_ParseQuotedField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            testSubject.Parse("foo,\"\"\",");

            Assert.Equal(1, testSubject.Fields.Count);
            Assert.Equal("foo,\"", testSubject.Fields[0]);
        }

        [Fact]
        public void DelimitedParseStateQuote_NewLine_ParseQuotedField()
        {
            var testSubject = DelimitedParserStub.Create<DelimitedParserStateQuote>(',');
            testSubject.Parse("f\r\noo\"");

            Assert.Equal(1, testSubject.Fields.Count);
            Assert.Equal("f\r\noo", testSubject.Fields[0]);
        }
    }
}
