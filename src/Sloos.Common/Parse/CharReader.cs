// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sloos.Common
{
    public sealed class CharReader : IEnumerator<char>, IEnumerable<char>
    {
        private const int BufferSize = 64 * 1024;

        private readonly char[] buffer;
        private readonly StreamReader reader;
        private readonly Stream stream;
        private int offset;
        private int size;

        public CharReader(Stream stream)
        {
            this.stream = stream;    
            this.reader = new StreamReader(stream);

            this.buffer = new char[CharReader.BufferSize];
        }

        public bool EndOfStream => this.ReadIntoBuffer() == 0;

        public IEnumerator<char> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        public char Current => this.buffer[this.offset];

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current => this.Current;

        public bool MoveNext()
        {
            bool canMoveNext = this.ReadIntoBuffer() > 0;
            ++this.offset;

            return canMoveNext;
        }

        public void Reset()
        {
            this.offset = 0;
            this.size = 0;
            this.stream.Seek(0, SeekOrigin.Begin);
        }

        public static CharReader From(string s)
        {
            return new CharReader(new MemoryStream(Encoding.UTF8.GetBytes(s)));
        }

        public void Close()
        {
            this.reader.Close();
            this.stream.Close();
        }

        private int ReadIntoBuffer()
        {
            if (this.IsEndOfBuffer())
            {
                this.size = this.reader.Read(this.buffer, 0, this.buffer.Length);
                this.offset = -1;
            }

            return this.size;
        }

        private bool IsEndOfBuffer()
        {
            return this.offset + 1 >= this.size;
        }
    }
}
