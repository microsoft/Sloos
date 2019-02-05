// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class SpreadsheetFactoryTest
    {
        [Fact]
        public void SpreadsheetShouldParseAllRows()
        {
            var delimiter = new Delimiter { DelimitedBy = ',' };

            var stream = "1,2\r\n3,4\r\n4,5".ToStream();
            var factory = new SpreadsheetFactory(delimiter);
    
            var testSubject = factory.Create(stream, 100);
            testSubject.Rows.Should().Be(3);
            testSubject.Columns.Should().Be(2);
        }

        [Fact]
        public void SpreadsheetShouldParseRequestedNumberOfRows()
        {
            var delimiter = new Delimiter { DelimitedBy = ',' };

            var stream = "1,2\r\n3,4\r\n4,5".ToStream();
            var factory = new SpreadsheetFactory(delimiter);

            var testSubject = factory.Create(stream, 1);
            testSubject.Rows.Should().Be(1);
            testSubject.Columns.Should().Be(2);
        }
    }
}
