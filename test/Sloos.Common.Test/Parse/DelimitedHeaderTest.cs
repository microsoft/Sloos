// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedHeaderTest
    {
        [Fact]
        public void DelimitedHeaderWithMultipleFields()
        {
            var factory = new DelimitedColumnFactory();
            var testSubject = new DelimitedHeader(
                new[] { "field1:int", "field2:bool", "field3" }.Select(factory.Create));

            Assert.Equal(typeof(int), testSubject.DelimitedColumns[0].Type);
            Assert.Equal(typeof(bool), testSubject.DelimitedColumns[1].Type);
            Assert.Equal(typeof(string), testSubject.DelimitedColumns[2].Type);

            Assert.Equal("field1", testSubject[0]);
            Assert.Equal("field2", testSubject[1]);
            Assert.Equal("field3", testSubject[2]);

            Assert.Equal(0, testSubject["field1"]);
            Assert.Equal(1, testSubject["field2"]);
            Assert.Equal(2, testSubject["field3"]);

            Assert.Equal(3, testSubject.DelimitedColumns.Length);
        }
    }
}
