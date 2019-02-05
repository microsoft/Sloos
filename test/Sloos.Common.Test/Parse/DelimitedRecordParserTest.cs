// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using System.Text;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedRecordParserTest
    {
        [Fact]
        public void RecordParser_SingleLine()
        {
            var reader = CharReader.From("a,b,c");
            var testSubject = new DelimitedRecordParser(reader, ',');

            var fields = testSubject.Parse();
            Assert.Equal(3, fields.Count());
        }

        [Fact]
        public void RecordParser_SingleLineCrlf()
        {
            var reader = CharReader.From("a,b,c\r\n");
            var testSubject = new DelimitedRecordParser(reader, ',');

            var fields = testSubject.Parse().ToArray();
            Assert.Equal(3, fields.Length);

            Assert.Equal("a", fields[0]);
            Assert.Equal("b", fields[1]);
            Assert.Equal("c", fields[2]);
        }

        [Fact]
        public void Blah()
        {
            var reader = CharReader.From("a\r\n");
            var testSubject = new DelimitedRecordParser(reader, ',');

            var fields = testSubject.Parse().ToArray();
            Assert.Equal(1, fields.Length);

            Assert.Equal("a", fields[0]);
        }

        [Fact]
        public void RecordParser_MultiLine()
        {
            var sb = new StringBuilder();
            sb.AppendLine("a,b,c");
            sb.Append("d,e,f");

            var reader = CharReader.From(sb.ToString());
            var testSubject1 = new DelimitedRecordParser(reader, ',');

            var fields1 = testSubject1.Parse().ToArray();
            Assert.Equal(3, fields1.Length);

            Assert.Equal("a", fields1[0]);
            Assert.Equal("b", fields1[1]);
            Assert.Equal("c", fields1[2]);

            var testSubject2 = new DelimitedRecordParser(reader, ',');
            var fields2 = testSubject2.Parse().ToArray();
            Assert.Equal(3, fields2.Length);

            Assert.Equal("d", fields2[0]);
            Assert.Equal("e", fields2[1]);
            Assert.Equal("f", fields2[2]);
        }

        [Fact]
        public void RecordParser_MultiLineCrlf()
        {
            var sb = new StringBuilder();
            sb.AppendLine("a,b,c");
            sb.AppendLine("d,e,f");

            var reader = CharReader.From(sb.ToString());
            var testSubject1 = new DelimitedRecordParser(reader, ',');

            var fields1 = testSubject1.Parse().ToArray();
            Assert.Equal(3, fields1.Length);

            Assert.Equal("a", fields1[0]);
            Assert.Equal("b", fields1[1]);
            Assert.Equal("c", fields1[2]);

            var testSubject2 = new DelimitedRecordParser(reader, ',');
            var fields2 = testSubject2.Parse().ToArray();
            Assert.Equal(3, fields2.Length);

            Assert.Equal("d", fields2[0]);
            Assert.Equal("e", fields2[1]);
            Assert.Equal("f", fields2[2]);
        }
    }
}
