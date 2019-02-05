// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class SpreadsheetTest
    {
        [Fact]
        public void DimensionsShouldBeCorrect()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "1", "2" },
                    new[] { "1", "3" },
                    new[] { "1", "4" },
                });

            spreadsheet.Rows.Should().Be(3);
            spreadsheet.Columns.Should().Be(2);

            spreadsheet.GetRow(0).Should().ContainInOrder("1", "2");
            spreadsheet.GetRow(2).Should().ContainInOrder("1", "4");
        }

        [Fact]
        public void ColumnApplyShouldOperateByColumns()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "1", "2" },
                    new[] { "1", "3" },
                    new[] { "1", "4" },
                });

            spreadsheet.ColumnApply(x => x.All(y => y == "1"), 0).Should().BeTrue();
            spreadsheet.ColumnApply(x => x.Select(int.Parse).Sum(), 1).Should().Be(9);
        }

        [Fact]
        public void ColumnApplyShouldSkipRows()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "1", "2" },
                    new[] { "1", "3" },
                    new[] { "1", "4" },
                });

            spreadsheet.ColumnApply(x => x.Select(int.Parse).Sum(), 1, 0).Should().Be(9);
            spreadsheet.ColumnApply(x => x.Select(int.Parse).Sum(), 1, 1).Should().Be(7);
            spreadsheet.ColumnApply(x => x.Select(int.Parse).Sum(), 1, 2).Should().Be(4);
        }
    }
}
