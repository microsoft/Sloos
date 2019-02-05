// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class SpreadsheetColumnTypeDetectorTest
    {
        [Fact]
        public void ElectorShouldDetermineCorrectTypes()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "AA", "BB" },
                    new[] { "1", "3" },
                    new[] { "1", "4" },
                });

            var testSubject = new SpreadsheetColumnTypeDetector();
            var columns = testSubject.Elect(spreadsheet)
                .ToArray();

            columns.Should().HaveCount(2);
            columns[0].TypeOfAllRows.Should().Be(typeof(string));
            columns[1].TypeOfAllRows.Should().Be(typeof(string));
            columns[0].TypeOfNonHeaderRows.Should().Be(typeof(int));
            columns[1].TypeOfNonHeaderRows.Should().Be(typeof(int));
        }    
    }
}
