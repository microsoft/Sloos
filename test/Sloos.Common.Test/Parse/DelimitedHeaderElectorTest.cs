// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedHeaderElectorTest
    {
        [Fact]
        public void Test()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "Col0", "Col1", "Col2" },
                    new[] { "a", "b", "c" },
                });

            var headerElector = new DelimitedHeaderElector();
            var header = headerElector.Elect(spreadsheet);
            header.Count.Should().Be(3);

            var columns = header.DelimitedColumns.ToArray();
            
            columns[0].Name.Should().Be("Col0");
            columns[1].Name.Should().Be("Col1");
            columns[2].Name.Should().Be("Col2");
            columns[0].Type.Should().Be(typeof(char));
            columns[1].Type.Should().Be(typeof(char));
            columns[2].Type.Should().Be(typeof(char));
        }

        [Fact]
        public void Test2()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "a", "b",   "c" },
                    new[] { "d", "101", "f" },
                    new[] { "d", "e",   "f" },
                });

            var headerElector = new DelimitedHeaderElector();
            var header = headerElector.Elect(spreadsheet);
            header.Count.Should().Be(3);

            var columns = header.DelimitedColumns.ToArray();
            columns[0].Name.Should().Be("Column0");
            columns[1].Name.Should().Be("Column1");
            columns[2].Name.Should().Be("Column2");
            columns[0].Type.Should().Be(typeof(char));
            columns[1].Type.Should().Be(typeof(string));
            columns[2].Type.Should().Be(typeof(char));
        }

        [Fact]
        public void Test3()
        {
            var spreadsheet = new Spreadsheet(
                new[]
                {
                    new[] { "aa", "bb",  },
                    new[] { "cc", "Z",  },
                });

            var headerElector = new DelimitedHeaderElector();
            var header = headerElector.Elect(spreadsheet);
            header.Count.Should().Be(2);

            var columns = header.DelimitedColumns.ToArray();
            columns[0].Name.Should().Be("aa");
            columns[1].Name.Should().Be("bb");
            columns[0].Type.Should().Be(typeof(string));
            columns[1].Type.Should().Be(typeof(char));
        }
    }
}
