// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Slam.Common;
using Slam.Cosmos.Log.Parse;
using Xunit;

namespace Slam.Cosmos.Log.Test.Parse
{
    public class HeaderElectorTest
    {
        [Fact]
        public void Test()
        {
            var input = "Col0,Col1,Col2\r\na,b,c";

            var headerElector = new HeaderElector(new Delimiter() { DelimitedBy = ',' });
            var header = headerElector.Elect(input.ToStream());
            header.Count.Should().Be(3);

            var columns = header.DelimitedColumns.ToArray();
            columns[0].Name.Should().Be("Col0");
            columns[0].Type.Should().Be(typeof(char));
            columns[1].Name.Should().Be("Col1");
            columns[1].Type.Should().Be(typeof(char));
            columns[2].Name.Should().Be("Col2");
            columns[2].Type.Should().Be(typeof(char));
        }

        [Fact]
        public void Test2()
        {
            var input = "a,b,c\r\nd,e,f";

            var headerElector = new HeaderElector(new Delimiter() { DelimitedBy = ',' });
            var header = headerElector.Elect(input.ToStream());
            header.Count.Should().Be(3);

            var columns = header.DelimitedColumns.ToArray();
            columns[0].Name.Should().Be("Column0");
            columns[0].Type.Should().Be(typeof(char));
            columns[1].Name.Should().Be("Column1");
            columns[1].Type.Should().Be(typeof(char));
            columns[2].Name.Should().Be("Column2");
            columns[2].Type.Should().Be(typeof(char));
        }

        [Fact]
        public void Test3()
        {
            var input = "aa,bb\r\ncc,Z";

            var headerElector = new HeaderElector(new Delimiter() { DelimitedBy = ',' });
            var header = headerElector.Elect(input.ToStream());
            header.Count.Should().Be(2);

            var columns = header.DelimitedColumns.ToArray();
            columns[0].Name.Should().Be("aa");
            columns[0].Type.Should().Be(typeof(string));
            columns[1].Name.Should().Be("bb");
            columns[1].Type.Should().Be(typeof(char));
        }
    }

    public class HeaderElector 
    {
        private readonly Delimiter delimiter;

        public HeaderElector(Delimiter delimiter)
        {
            this.delimiter = delimiter;
        }

        public DelimitedHeader Elect(Stream stream)
        {
            var table = this.ParseBeginningOfFile(
                new CharReader(stream),
                this.delimiter)
                .ToArray();

            var typeElector = new TypeElector();
            var electionResults = new List<Tuple<Type, Type>>();

            for (int i = 0; i < table[0].Length; i++)
            {
                var typeOfAllColumns = typeElector.Elect(table.Select(x => x[i]));
                var typeOfColumnsMinusHeader = typeElector.Elect(table.Skip(1).Select(x => x[i]));

                electionResults.Add(
                    Tuple.Create(typeOfAllColumns, typeOfColumnsMinusHeader));
            }

            var hasHeader = electionResults.Any(x => x.Item1 != x.Item2);
            if (hasHeader)
            {
                var columnNames = table[0];
                return new DelimitedHeader(
                    columnNames.Zip(electionResults, (x, y) => new DelimitedColumn() { Name = x, Type = y.Item2 }));
            }

            return new DelimitedHeader(
                electionResults.Select((x, i) => new DelimitedColumn() { Name = String.Format("Column{0}", i), Type = x.Item1 }));
        }

        private IEnumerable<string[]> ParseBeginningOfFile(CharReader reader, Delimiter delimiter)
        {
            int count = 0;
            while (!reader.EndOfStream && count++ < 100)
            {
                var fields = new DelimitedRecordParser(reader, delimiter.DelimitedBy).Parse().ToArray();
                yield return fields.ToArray();
            }
        }
    }
}
