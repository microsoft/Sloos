// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sloos.Common
{
    public sealed class DelimiterElector
    {
        private readonly int rowsToRead;
        private static readonly char[] Delimiters = { ',', '\t', '|', ' ', ';', ':' };

        public DelimiterElector(int rowsToRead)
        {
            this.rowsToRead = rowsToRead;
        }

        public Delimiter Elect(Stream stream)
        {
            var reader = new CharReader(stream);
            var map = new Dictionary<char, int>();

            foreach (var delimiter in DelimiterElector.Delimiters)
            {
                reader.Reset();

                var arrayOfColumnCounts = this.GetCountOfFields(reader, delimiter).ToArray();
                var columnCount = arrayOfColumnCounts[0];

                if (this.AllRowsHaveSameNumberOfColumns(arrayOfColumnCounts) && 
                    columnCount > 1)
                {
                    map[delimiter] = arrayOfColumnCounts[0];
                }
            }

            this.AssertDelimiterElected(map.Any());

            var electedDelimiter = map.OrderByDescending(x => x.Value).First().Key;
            return new Delimiter { DelimitedBy = electedDelimiter };
        }

        private void AssertDelimiterElected(bool any)
        {
            if (!any)
            {
                string message = "Could not determine an appropriate delimiter!";
                throw new ArgumentOutOfRangeException(message);
            }
        }

        private bool AllRowsHaveSameNumberOfColumns(int[] xs)
        {
            return xs.All(x => xs[0] == x);
        }

        private IEnumerable<int> GetCountOfFields(CharReader reader, char delimiter)
        {
            int count = 0;
            while (!reader.EndOfStream && count++ < this.rowsToRead)
            {
                var fields = new DelimitedRecordParser(reader, delimiter).Parse().ToArray();
                yield return fields.Length;
            }
        }
    }
}
