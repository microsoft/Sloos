// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Sloos.Common.Test
{
    using System.Collections.Generic;

    public class CsvSerializerTest
    {
        [Fact]
        public void CsvSerializer_NonDataContract_Skips()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                Action testSubject = () => new CsvSerializer<TestData.ClassWithoutDataContractAttribute>();
                testSubject.Invoke();

                Assert.True(false, "Did not expect to execute this statement.");
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [Fact]
        public void CsvSerializer_MapObjectArrayToSerializeClass()
        {
            var values = new[] { "field" };
            var testSubject = new CsvSerializer<TestData.Simple>();

            var row = testSubject.Deserialize(TestHelpers.StringArrayToStream(values)).First();

            Assert.Equal(default(int), row.ID);
            Assert.Equal("field", row.Field);
        }

        [Fact]
        public void CsvSerializer_ColumnNames_Respect_OrderAttribute()
        {
            var testSubject = new CsvSerializer<TestData.ExplicitlyOrdered>();
            Assert.Equal("Field0", testSubject.ColumnNames[0]);
            Assert.Equal("Field1", testSubject.ColumnNames[1]);
            Assert.Equal("Field2", testSubject.ColumnNames[2]);
            Assert.Equal("Field3", testSubject.ColumnNames[3]);
        }

        [Fact]
        public void CsvSerializer_ColumnNames_Respect_DefinitionOrder()
        {
            var testSubject = new CsvSerializer<TestData.ImplicitlyOrdered>();
            Assert.Equal("Field1", testSubject.ColumnNames[0]);
            Assert.Equal("Field2", testSubject.ColumnNames[1]);
            Assert.Equal("Field3", testSubject.ColumnNames[2]);
        }

        [Fact]
        public void CsvSerializer_ParseDocument()
        {
            var sb = new StringBuilder();
            sb.AppendLine("a,b,c");
            sb.AppendLine("a,b,c");
            sb.AppendLine("a,b,c");

            var testSubject = new CsvSerializer<TestData.ImplicitlyOrdered>();
            var rows = testSubject.Deserialize(sb.ToStream())
                .ToArray();

            Assert.True(rows.All(x => x.Field1 == "a"));
            Assert.True(rows.All(x => x.Field2 == "b"));
            Assert.True(rows.All(x => x.Field3 == "c"));
            Assert.Equal(3, rows.Length);
        }

        [Fact]
        public void CsvSerializer_IterateOverResults()
        {
            var sb = new StringBuilder();
            sb.AppendLine("a,b,c");
            sb.AppendLine("d,e,f");
            sb.AppendLine("g,h,i");

            var testSubject = new CsvSerializer<TestData.ImplicitlyOrdered>();
            var rows = new List<string>();

            using (var stream = sb.ToStream())
            {
                rows.AddRange(
                    testSubject.Deserialize(stream)
                        .Select(row => $"{row.Field1},{row.Field2},{row.Field3}"));
            }

            Assert.Equal(3, rows.Count);

            Assert.Equal("a,b,c", rows[0]);
            Assert.Equal("d,e,f", rows[1]);
            Assert.Equal("g,h,i", rows[2]);
        }
    }
}
