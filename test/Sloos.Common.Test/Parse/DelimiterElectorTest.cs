// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimiterElectorTest
    {
        [Fact]
        public void ElectorShouldParseCommaDelimitedColumns()
        {
            var testSubject = new DelimiterElector(100);
            var delimiter = testSubject.Elect("a,b,c".ToStream());
            delimiter.DelimitedBy.Should().Be(',');
        }

        [Fact]
        public void ElectorShouldThrowWhenColumnCountsAreMismatched()
        {
            var testSubject = new DelimiterElector(100);
            Action test = () => testSubject.Elect("a,b,c\r\na,b".ToStream());

            test.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ElectorShouldParseTabDelimitedColumns()
        {
            var testSubject = new DelimiterElector(100);
            var delimiter = testSubject.Elect("a\tb\tc".ToStream());
            delimiter.DelimitedBy.Should().Be('\t');
        }

        [Fact]
        public void ElectorShouldParseSpaceDelimitedColumns()
        {
            var testSubject = new DelimiterElector(100);
            var delimiter = testSubject.Elect("a b c".ToStream());
            delimiter.DelimitedBy.Should().Be(' ');
        }

        [Fact]
        public void ElectorShouldParsePipeDelimitedColumns()
        {
            var testSubject = new DelimiterElector(100);
            var delimiter = testSubject.Elect("a|b|c".ToStream());
            delimiter.DelimitedBy.Should().Be('|');
        }

        [Fact]
        public void ElectorShouldParseSemicolonDelimitedColumns()
        {
            var testSubject = new DelimiterElector(100);
            var delimiter = testSubject.Elect("a;b;c".ToStream());
            delimiter.DelimitedBy.Should().Be(';');
        }

        [Fact]
        public void ElectorShouldParseColonDelimitedColumns()
        {
            var testSubject = new DelimiterElector(100);
            var delimiter = testSubject.Elect("a:b:c".ToStream());
            delimiter.DelimitedBy.Should().Be(':');
        }
    }
}
