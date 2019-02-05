// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedColumnFactoryTest
    {
        [Fact]
        public void ParseKnownPrimitiveTypeNames()
        {
            var testSubject = new DelimitedColumnFactory();

            testSubject.Create("field1:bool").Type.Should().Be(typeof(bool));
            testSubject.Create("field1:bool?").Type.Should().Be(typeof(bool?));
            testSubject.Create("field1:byte").Type.Should().Be(typeof(byte));
            testSubject.Create("field1:byte?").Type.Should().Be(typeof(byte?));
            testSubject.Create("field1:char").Type.Should().Be(typeof(char));
            testSubject.Create("field1:char?").Type.Should().Be(typeof(char?));
            testSubject.Create("field1:DateTime").Type.Should().Be(typeof(DateTime));
            testSubject.Create("field1:DateTime?").Type.Should().Be(typeof(DateTime?));
            testSubject.Create("field1:DateTimeOffset").Type.Should().Be(typeof(DateTimeOffset));
            testSubject.Create("field1:DateTimeOffset?").Type.Should().Be(typeof(DateTimeOffset?));
            testSubject.Create("field1:decimal").Type.Should().Be(typeof(decimal));
            testSubject.Create("field1:decimal?").Type.Should().Be(typeof(decimal?));
            testSubject.Create("field1:double").Type.Should().Be(typeof(double));
            testSubject.Create("field1:double?").Type.Should().Be(typeof(double?));
            testSubject.Create("field1:float").Type.Should().Be(typeof(float));
            testSubject.Create("field1:float?").Type.Should().Be(typeof(float?));
            testSubject.Create("field1:Guid").Type.Should().Be(typeof(Guid));
            testSubject.Create("field1:Guid?").Type.Should().Be(typeof(Guid?));
            testSubject.Create("field1:short").Type.Should().Be(typeof(short));
            testSubject.Create("field1:short?").Type.Should().Be(typeof(short?));
            testSubject.Create("field1:int").Type.Should().Be(typeof(int));
            testSubject.Create("field1:int?").Type.Should().Be(typeof(int?));
            testSubject.Create("field1:long").Type.Should().Be(typeof(long));
            testSubject.Create("field1:long?").Type.Should().Be(typeof(long?));
            testSubject.Create("field1:string").Type.Should().Be(typeof(string));
            testSubject.Create("field1:timespan").Type.Should().Be(typeof(TimeSpan));
            testSubject.Create("field1:timespan?").Type.Should().Be(typeof(TimeSpan?));
        }

        [Fact]
        public void FactoryShouldDefaultToString()
        {
            var testSubject = new DelimitedColumnFactory();
            testSubject.Create("field").Type.Should().Be(typeof(string));
        }

        [Fact]
        public void UnknownTypeShouldThrowAnException()
        {
            var testSubject = new DelimitedColumnFactory();
            Action test = () => testSubject.Create("field:_unknown_type_");

            test.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void FactoryEnumerableShouldDefaultToString()
        {
            var testSubject = new DelimitedColumnFactory();
            testSubject.Create(Enumerable.Repeat("field", 10))
                .All(x => x.Type == typeof(string))
                .Should()
                .BeTrue();
        }
    }
}
