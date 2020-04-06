// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedParserTest
    {
        [Fact]
        public void DelimitedParser_ParseNonDataReader()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b",
                new[] { "field1:char", "field2:char?" },
                ',');

            var results = testSubject.Parse().ToArray();
            Assert.Single(results);

            var record = results.First();
            Assert.Equal(2, record.Count());

            testSubject.Dispose();
        }

        [Fact]
        public void DelimitedParser_DataReader_Basic()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b",
                new[] { "field1:char", "field2:char?" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(2, testSubject.FieldCount);
            Assert.Equal("a", testSubject[0]);
            Assert.Equal("b", testSubject[1]);

            Assert.Equal(0, testSubject.GetOrdinal("field1"));
            Assert.Equal(1, testSubject.GetOrdinal("field2"));

            Assert.Equal("a", testSubject["field1"].ToString());
            Assert.Equal("b", testSubject["field2"].ToString());

            Assert.Equal("field1", testSubject.GetName(0));
            Assert.Equal("field2", testSubject.GetName(1));

            Assert.Equal(typeof(char), testSubject.GetFieldType(0));
            Assert.Equal(typeof(char?), testSubject.GetFieldType(1));

            Assert.False(testSubject.IsDBNull(0));
            Assert.False(testSubject.IsDBNull(1));

            Assert.Equal("Char", testSubject.GetDataTypeName(0));
            Assert.Equal("Nullable`1", testSubject.GetDataTypeName(1));

            testSubject.Close();
            Assert.True(testSubject.IsClosed);
        }

        // XXX: I am no longer sure this is a valid scenario.
        //
        //[Fact]
        //public void DelimitedParser_DataReaderAutoParseHeader()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("field1:char,field2:char?");
        //    sb.AppendLine("a,b");

        //    var stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        //    DelimitedParser testSubject = DelimitedParser.Create(stream, ',');

        //    Assert.True(testSubject.Read());

        //    Assert.Equal(2, testSubject.FieldCount);
        //    Assert.Equal("a", testSubject[0]);
        //    Assert.Equal("b", testSubject[1]);

        //    Assert.Equal(0, testSubject.GetOrdinal("field1"));
        //    Assert.Equal(1, testSubject.GetOrdinal("field2"));

        //    Assert.Equal("a", testSubject["field1"].ToString());
        //    Assert.Equal("b", testSubject["field2"].ToString());

        //    Assert.Equal("field1", testSubject.GetName(0));
        //    Assert.Equal("field2", testSubject.GetName(1));

        //    Assert.Equal(typeof(char), testSubject.GetFieldType(0));
        //    Assert.Equal(typeof(char?), testSubject.GetFieldType(1));

        //    Assert.False(testSubject.IsDBNull(0));
        //    Assert.False(testSubject.IsDBNull(1));

        //    Assert.Equal("Char", testSubject.GetDataTypeName(0));
        //    Assert.Equal("Nullable`1", testSubject.GetDataTypeName(1));

        //    testSubject.Close();
        //    Assert.True(testSubject.IsClosed);
        //}

        [Fact]
        public void DataReader_EmptyStream_ReturnsNoData()
        {
            var testSubject = this.CreateDelimitedParser(
                "",
                new[] { "field1" },
                ',');

            Assert.False(testSubject.Read());
            Assert.False(testSubject.NextResult());
        }

        [Fact]
        public void DataReader_SingleLineByIndex_ReadData_()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(3, testSubject.FieldCount);
            Assert.Equal("a", testSubject[0].ToString());
            Assert.Equal("b", testSubject[1].ToString());
            Assert.Equal("c", testSubject[2].ToString());

            Assert.False(testSubject.IsDBNull(0));
        }

        [Fact]
        public void DataReader_AccessOutOfBounds_Throws()
        {
            this.AssertOutOfBoundsAccess(x => x.GetChar(100));
            this.AssertOutOfBoundsAccess(x => x.GetDateTime(100));
            this.AssertOutOfBoundsAccess(x => x.GetDecimal(100));
            this.AssertOutOfBoundsAccess(x => x.GetDouble(100));
            this.AssertOutOfBoundsAccess(x => x.GetFloat(100));
            this.AssertOutOfBoundsAccess(x => x.GetGuid(100));
            this.AssertOutOfBoundsAccess(x => x.GetInt16(100));
            this.AssertOutOfBoundsAccess(x => x.GetInt32(100));
            this.AssertOutOfBoundsAccess(x => x.GetInt64(100));
            this.AssertOutOfBoundsAccess(x => x.GetString(100));
            this.AssertOutOfBoundsAccess(x => x.GetValue(100));
        }

        private void AssertOutOfBoundsAccess(Action<DelimitedParser> action)
        {
            var testSubject = this.CreateDelimitedParser(
                "a",
                new[] { "field1" },
                ',');

            Assert.True(testSubject.Read());

            try
            {
                action(testSubject);
                Assert.False(true, "Did not expect to execute this line!");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        //[Fact]
        [Fact(Skip = "Debug only.")]
        [Trait("IsInteractive", "true")]
        public void ParseHonkingBigFile()
        {
            Stopwatch sw = new Stopwatch();
            using (var stream = File.OpenRead(@"c:\temp\test1.csv"))
            {
                sw.Restart();
                long count = 0;

                var settings = this.GetDelimitedParserSettings(
                    new[] { "field1", "field2" },
                    ',');

                var parser = DelimitedParser.Create(settings, stream);

                //foreach (var record in parser.Parse(stream))
                //{
                //    count += record.LongCount();
                //}

                //var a = parser.Parse().Last();

                sw.Stop();
                var elapsed = sw.Elapsed;
                var s = $"elapsed={elapsed}, count={count}, throughput={(stream.Length / (1024 * 1024)) / elapsed.TotalSeconds} MB/s";
                Console.WriteLine(s);
            }
        }

        #region Get Index by Type

        [Fact]
        public void DataReader_SingleLineGetBoolean_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "true,TRUE,false,FALSE",
                new[] { "field1", "field2", "field3", "field4" },
                ',');

            Assert.True(testSubject.Read());
            Assert.True(testSubject.GetBoolean(0));
            Assert.True(testSubject.GetBoolean(1));
            Assert.False(testSubject.GetBoolean(2));
            Assert.False(testSubject.GetBoolean(3));
        }

        [Fact]
        public void DataReader_SingleLineGetByte_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "0,1",
                new[] { "field1", "field2" },
                ',');

            Assert.True(testSubject.Read());
            Assert.Equal(0, testSubject.GetByte(0));
            Assert.Equal(1, testSubject.GetByte(1));
        }

        [Fact]
        public void DataReader_SingleLineGetBytes_NotImplemented()
        {
            var testSubject = this.CreateDelimitedParser(
                "0,1",
                new[] { "field1", "field2" },
                ',');

            try
            {
                testSubject.GetBytes(0, 0, null, 0, 0);
                Assert.True(false, "Did not expect to execute this statement!");
            }
            catch (NotImplementedException)
            {
                // Expected
            }
        }

        [Fact]
        public void DataReader_SingleLineGetChar_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());
            Assert.Equal('a', testSubject.GetChar(0));
            Assert.Equal('b', testSubject.GetChar(1));
            Assert.Equal('c', testSubject.GetChar(2));
        }

        [Fact]
        public void DataReader_SingleLineGetChars_NotImplemented()
        {
            var testSubject = this.CreateDelimitedParser(
                "0,1",
                new[] { "field1", "field2" },
                ',');

            try
            {
                testSubject.GetChars(0, 0, null, 0, 0);
                Assert.True(false, "Did not expect to execute this statement!");
            }
            catch (NotImplementedException)
            {
                // Expected
            }
        }

        [Fact]
        public void DataReader_SingleLineGetData_NotImplemented()
        {
            var testSubject = this.CreateDelimitedParser(
                "0,1",
                new[] { "field1", "field2" },
                ',');

            try
            {
                testSubject.GetData(0);
                Assert.True(false, "Did not expect to execute this statement!");
            }
            catch (NotImplementedException)
            {
                // Expected
            }
        }

        [Fact]
        public void DataReader_SingleLineGetDateTime_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "2000-01-01,10/31/2012 12:24:48 PM,2005-07-04T10:20:30.1250000Z",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            var dateTime1 = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var dateTime2 = new DateTime(2012, 10, 31, 12, 24, 48, DateTimeKind.Utc);
            var dateTime3 = new DateTime(2005, 7, 4, 10, 20, 30, 125, DateTimeKind.Utc);

            Assert.Equal(dateTime1, testSubject.GetDateTime(0));
            Assert.Equal(dateTime2, testSubject.GetDateTime(1));
            Assert.Equal(dateTime3, testSubject.GetDateTime(2));
        }

        [Fact]
        public void DataReader_SingleLineGetDecimal_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "1.01,0.2,3",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(1.01m, testSubject.GetDecimal(0));
            Assert.Equal(0.2m, testSubject.GetDecimal(1));
            Assert.Equal(3m, testSubject.GetDecimal(2));
        }

        [Fact]
        public void DataReader_SingleLineGetDouble_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "1.01,0.2,3",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(1.01d, testSubject.GetDouble(0));
            Assert.Equal(0.2d, testSubject.GetDouble(1));
            Assert.Equal(3d, testSubject.GetDouble(2));
        }

        [Fact]
        public void DataReader_SingleLineGetFloat_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "1.01,0.2,3",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(1.01f, testSubject.GetFloat(0));
            Assert.Equal(0.2f, testSubject.GetFloat(1));
            Assert.Equal(3f, testSubject.GetFloat(2));
        }

        [Fact]
        public void DataReader_SingleLineGetGuid_ReadData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("01234567-feed-face-beed-0123456789ab,");                                     // .ToString("d")
            sb.Append("01234567feedfacebeed0123456789ab,");                                         // .ToString("n")
            sb.Append("(01234567-feed-face-beed-0123456789ab),");                                   // .ToString("p")
            sb.Append("{01234567-feed-face-beed-0123456789ab},");                                   // .ToString("b")
            sb.Append(@"""{0x01234567,0xfeed,0xface,{0xbe,0xed,0x01,0x23,0x45,0x67,0x89,0xab}}"""); // .ToString("x")

            var testSubject = this.CreateDelimitedParser(
                sb.ToString(),
                new[] { "field1", "field2", "field3", "field4", "field5" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal("01234567-feed-face-beed-0123456789ab", testSubject.GetGuid(0).ToString());
            Assert.Equal("01234567-feed-face-beed-0123456789ab", testSubject.GetGuid(1).ToString());
            Assert.Equal("01234567-feed-face-beed-0123456789ab", testSubject.GetGuid(2).ToString());
            Assert.Equal("01234567-feed-face-beed-0123456789ab", testSubject.GetGuid(3).ToString());
            Assert.Equal("01234567-feed-face-beed-0123456789ab", testSubject.GetGuid(4).ToString());
        }

        [Fact]
        public void DataReader_SingleLineGetInt16_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "1,2,3",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(1, testSubject.GetInt16(0));
            Assert.Equal(2, testSubject.GetInt16(1));
            Assert.Equal(3, testSubject.GetInt16(2));
        }

        [Fact]
        public void DataReader_SingleLineGetInt32_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "1,2,3",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(1, testSubject.GetInt32(0));
            Assert.Equal(2, testSubject.GetInt32(1));
            Assert.Equal(3, testSubject.GetInt32(2));
        }

        [Fact]
        public void DataReader_SingleLineGetInt64_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "1,2,3",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            Assert.Equal(1, testSubject.GetInt64(0));
            Assert.Equal(2, testSubject.GetInt64(1));
            Assert.Equal(3, testSubject.GetInt64(2));
        }

        [Fact]
        public void DataReader_SingleLineGetString_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());
            Assert.Equal("a", testSubject.GetString(0));
            Assert.Equal("b", testSubject.GetString(1));
            Assert.Equal("c", testSubject.GetString(2));
        }

        [Fact]
        public void DataReader_SingleLineGetValue_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());
            Assert.Equal("a", testSubject.GetValue(0));
            Assert.Equal("b", testSubject.GetValue(1));
            Assert.Equal("c", testSubject.GetValue(2));
        }

        [Fact]
        public void DataReader_GetValues_ReadData()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.True(testSubject.Read());

            var data = new object[3];
            int count = testSubject.GetValues(data);

            Assert.Equal(3, count);
            Assert.Equal("a", data[0]);
            Assert.Equal("b", data[1]);
            Assert.Equal("c", data[2]);
        }

        [Fact]
        public void DataReader_Depth_IsAlwaysOne()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.Equal(1, testSubject.Depth);
        }

        [Fact]
        public void DataReader_RecordsAffected_IsAlwaysNegativeOne()
        {
            var testSubject = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            Assert.Equal(-1, testSubject.RecordsAffected);
        }

        [Fact]
        public void DataReader_IsDBNull()
        {
            var testSubject = this.CreateDelimitedParser(
                ",,",
                new[] { "field1", "field2:int", "field3:int?" },
                ',');

            Assert.True(testSubject.IsDBNull(0));
            Assert.False(testSubject.IsDBNull(1));
            Assert.True(testSubject.IsDBNull(2));
        }

        #endregion Get Index by Type

        #region Not Implemented

        [Fact]
        public void DataReader_GetSchemaTable_NotImplemented()
        {
            this.AssertNotImplemented(x => x.GetSchemaTable());
        }

        private void AssertNotImplemented(Action<DelimitedParser> action)
        {
            var parser = this.CreateDelimitedParser(
                "a,b,c",
                new[] { "field1", "field2", "field3" },
                ',');

            try
            {
                action(parser);
                Assert.False(true, "Did not expect to execute this statement!");
            }
            catch (NotImplementedException)
            {
                // Expected
            }
        }

        #endregion Not Implemented

        private DelimitedParser CreateDelimitedParser(
            string input,
            IEnumerable<string> delimitedColumnStrings,
            char delimiter)
        {
            var stream = input.ToStream();
            var settings = this.GetDelimitedParserSettings(delimitedColumnStrings, delimiter);

            DelimitedParser parser = DelimitedParser.Create(settings, stream);
            return parser;
        }

        private DelimitedParserSettings GetDelimitedParserSettings(
            IEnumerable<string> delimitedColumnStrings,
            char delimiter)
        {
            var factory = new DelimitedColumnFactory();
            var columns = factory.Create(delimitedColumnStrings);

            var settings = new DelimitedParserSettings()
            {
                DelimitedHeader = new DelimitedHeader(columns),
                Delimiter = delimiter,
            };

            return settings;
        }
    }
}
