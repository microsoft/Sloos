// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sloos.Common
{
    public sealed class SpreadsheetFactory 
    {
        private readonly Delimiter delimiter;

        public SpreadsheetFactory(Delimiter delimiter)
        {
            this.delimiter = delimiter;
        }

        public Spreadsheet Create(Stream stream, int rowsToRead)
        {
            var reader = new CharReader(stream);
            var grid = this.ReadRows(reader)
                .Take(rowsToRead)
                .ToArray();

            var spreadsheet = new Spreadsheet(grid);
            return spreadsheet;
        }

        private IEnumerable<string[]> ReadRows(CharReader reader)
        {
            while (!reader.EndOfStream)
            {
                yield return new DelimitedRecordParser(reader, this.delimiter.DelimitedBy).Parse().ToArray();
            }
        }
    }
}
