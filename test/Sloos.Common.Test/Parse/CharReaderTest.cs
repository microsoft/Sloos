// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Sloos.Common.Test
{
    public class CharReaderTest
    {
        private const string Sentinel = "0123456789";

        [Fact]
        public void CharReader_Read()
        {
            var reader = this.CreateCharReader();
            Assert.False(reader.EndOfStream);

            var chars = reader.ToArray();
            Assert.Equal(10, chars.Length);
            Assert.Equal('0', chars[0]);
            Assert.Equal('9', chars[9]);

            Assert.True(reader.EndOfStream);
        }

        [Fact]
        public void CharReader_CodeCoverage()
        {
            var reader = this.CreateCharReader();
            Assert.NotNull(reader.Current);

            var enumerable = reader as System.Collections.IEnumerable;
            Assert.NotNull(enumerable);
            Assert.NotNull(enumerable.GetEnumerator());

            var enumerator = reader as System.Collections.IEnumerator;
            Assert.NotNull(enumerator.Current);
        }

        [Fact]
        public void ResetShouldResetStreamPosition()
        {
            var stream = CharReaderTest.Sentinel.ToStream();
            var reader = new CharReader(stream);

            stream.Position.Should().Be(0);
            reader.Take(3).Should().ContainInOrder('0', '1', '2');
            stream.Position.Should().NotBe(0);

            reader.Reset();
            stream.Position.Should().Be(0);
            reader.Take(3).Should().ContainInOrder('0', '1', '2');
        }

        [Fact]
        public void CharReader_Stateful_Read()
        {
            var reader = this.CreateCharReader();

            var chars1 = reader.Take(5).ToArray();
            var chars2 = reader.Take(5).ToArray();

            Assert.Equal(5, chars1.Length);
            Assert.Equal('0', chars1[0]);
            Assert.Equal('4', chars1[4]);

            Assert.Equal(5, chars2.Length);
            Assert.Equal('5', chars2[0]);
            Assert.Equal('9', chars2[4]);
        }

        [Fact]
        public void CharReader_Eof_Read()
        {
            var chars = this.CreateCharReader().Take(1000).ToArray();
            Assert.Equal(10, chars.Length);
        }

        [Fact]
        public void CharReader_MultipleRead()
        {
            const int count = 72 * 1000;
            
            var sb = new StringBuilder(count);
            Enumerable.Range(0, count / 10).ToList().ForEach(x => sb.Append(CharReaderTest.Sentinel));

            var reader = new CharReader(sb.ToStream());

            Assert.Equal(count, reader.Count());
        }

        [Fact]
        public void CharReader_AlignedBufferRead_NonPrematureEndOfStream()
        {
            const int count = 128 * 1024;
            const string line = "abc,abc,abc,abc,abc,abc,abc,abc,abc,abc,abc,abc,abc,abc,abc,ab\r\n";

            Assert.Equal(64, line.Length);

            var sb = new StringBuilder(count);
            Enumerable.Range(0, 1024 * 2).ToList().ForEach(x => sb.Append(line));

            var reader = new CharReader(sb.ToStream());

            Assert.False(reader.EndOfStream);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            reader.Take(count / 2).ToArray();
            Assert.False(reader.EndOfStream);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            reader.Take(count / 2).ToArray();
            Assert.True(reader.EndOfStream);
        }

        private CharReader CreateCharReader()
        {
            var reader = new CharReader(
                CharReaderTest.Sentinel.ToStream());

            return reader;
        }
    }
}
