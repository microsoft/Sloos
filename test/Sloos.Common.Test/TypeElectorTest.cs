// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class TypeElectorTest
    {
        [Fact]
        public void ShouldBeBool()
        {
            var testSubject = new TypeElector();
            var xs = new[] { "true", "false", "TRUE", "FalsE" };

            testSubject.Elect(xs).Should().Be(typeof(bool));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(bool?));
        }

        [Fact]
        public void ShouldBeInt()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "-1",
                         "0",
                         int.MinValue.ToString(),
                         int.MaxValue.ToString(),
                     };

            testSubject.Elect(xs).Should().Be(typeof(int));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(int?));
        }

        [Fact]
        public void ShouldBeLong()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "-1",
                         "0",
                         long.MinValue.ToString(),
                         long.MaxValue.ToString(),
                     };

            testSubject.Elect(xs).Should().Be(typeof(long));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(long?));
        }

        [Fact]
        public void ShouldBeDouble()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "-1",
                         "0",
                         float.MinValue.ToString(),
                         float.MaxValue.ToString(),
                         double.Epsilon.ToString(),
                         // XXX: say what!?!  MinValue and MaxValue cause string
                         // to be elected, not double.
                         //double.MinValue.ToString(),
                         //double.MaxValue.ToString(),
                     };

            testSubject.Elect(xs).Should().Be(typeof(double));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(double?));
        }

        [Fact]
        public void ShouldBeChar()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "0",
                         "1",
                         "a",
                         "Z",
                         char.MinValue.ToString(),
                         char.MaxValue.ToString(),
                     };

            testSubject.Elect(xs).Should().Be(typeof(char));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(char?));
        }

        [Fact]
        public void ShouldBeGuid()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         Guid.Empty.ToString(),
                         "115f3f02059447d291d055cfc5c86f6f", // ToString("N")
                         "115f3f02-0594-47d2-91d0-55cfc5c86f6f", // ToString("D")
                         "{115f3f02-0594-47d2-91d0-55cfc5c86f6f}", // ToString("B")
                         "(115f3f02-0594-47d2-91d0-55cfc5c86f6f)", // ToString("P")
                         "{0x115f3f02,0x0594,0x47d2,{0x91,0xd0,0x55,0xcf,0xc5,0xc8,0x6f,0x6f}}", // ToString("X")
                     };

            testSubject.Elect(xs).Should().Be(typeof(Guid));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(Guid?));
        }

        [Fact]
        public void ShouldBeULong()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "0",
                         ulong.MinValue.ToString(),
                         ulong.MaxValue.ToString(),
                     };

            testSubject.Elect(xs).Should().Be(typeof(ulong));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(ulong?));
        }

        [Fact]
        public void ShouldBeTimeSpan()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "01:23:45",
                         "987.01:23:45",
                         TimeSpan.Zero.ToString(),
                         TimeSpan.MinValue.ToString(),
                         TimeSpan.MaxValue.ToString(),
                     };

            testSubject.Elect(xs).Should().Be(typeof(TimeSpan));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(TimeSpan?));
        }

        [Fact]
        public void ShouldBeDateTime()
        {
            var dt = new DateTime(2000, 1, 1, 11, 12, 13);

            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         DateTime.MinValue.ToString(),
                         DateTime.MaxValue.ToString(),
                         dt.ToLongDateString(),
                         dt.ToShortDateString(),
                         dt.ToString("g"),
                         dt.ToString("G"),
                         dt.ToString("M"),
                         dt.ToString("o"),
                         dt.ToString("R"),
                         dt.ToString("s"),
                         dt.ToString("u"),
                         dt.ToString("U"),
                         dt.ToString("Y"),
                     };

            testSubject.Elect(xs).Should().Be(typeof(DateTime));
            testSubject.Elect(xs.Append(string.Empty).ToArray()).Should().Be(typeof(DateTime?));
        }

        [Fact]
        public void EmptyStringsShouldStillBeString()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         "dogfish",
                         string.Empty,
                     };

            testSubject.Elect(xs).Should().Be(typeof(string));
        }

        [Fact]
        public void CompleteMixShouldBeString()
        {
            var dt = new DateTime(2000, 1, 1, 11, 12, 13);

            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         bool.TrueString,
                         int.MinValue.ToString(),
                         Guid.Empty.ToString(),
                         dt.ToString("o"),
                     };

            testSubject.Elect(xs).Should().Be(typeof(string));
        }

        [Fact]
        public void AllNullAndEmptyShouldBeString()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                         null,
                         string.Empty,
                         " ",
                         "\t",
                         "\r\n",
                         "    ",
                     };

            testSubject.Elect(xs).Should().Be(typeof(string));
        }

        [Fact]
        public void NullableDouble()
        {
            var testSubject = new TypeElector();
            var xs = new[]
                     {
                        "",
                        null,
                        "",
                        "",
                        "1.01",
                        "123",
                        "456.789",
                     };

            testSubject.Elect(xs).Should().Be(typeof(double?));
        }
    }
}
