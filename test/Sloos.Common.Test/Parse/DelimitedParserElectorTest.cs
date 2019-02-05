// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class DelimitedParserElectorTest
    {
        [Fact]
        public void ElectorShouldElectDelimiterAndHeader()
        {
            var stream = "Dog,Cat\r\n1,2\r\n3,4.0".ToStream();
            var testSubject = new DelimitedParserElector(
                stream);

            testSubject.Delimiter.DelimitedBy.Should().Be(',');
            testSubject.Header.Count.Should().Be(2);
            testSubject.Header.DelimitedColumns[0].Name.Should().Be("Dog");
            testSubject.Header.DelimitedColumns[1].Name.Should().Be("Cat");

            testSubject.Header.DelimitedColumns[0].Type.Should().Be(typeof(int));
            testSubject.Header.DelimitedColumns[1].Type.Should().Be(typeof(double));
        }

        [Fact]
        public void StreamShouldBeReturnedZeroPosition()
        {
            var stream = "Dog,Cat\r\n1,2\r\n3,4.0".ToStream();
            var testSubject = new DelimitedParserElector(
                stream);

            testSubject.Delimiter.DelimitedBy.Should().Be(',');
            stream.Position.Should().Be(0);
        }

        [Fact]
        public void ElectorHasNoClue()
        {
            var stream = "Dog,Cat,Bark\r\n1,2\r\n3,4.0".ToStream();
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new DelimitedParserElector(stream);

            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}
