// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.IO;

namespace Sloos.Common
{
    public sealed class DelimitedParserElector
    {
        public const int MaxRowsToRead = 500;

        public DelimitedParserElector(Stream stream)
        {
            this.ResetStream(stream);
            this.Delimiter = this.ElectDelimiter(stream);
            this.ResetStream(stream);
            this.Header = this.ElectHeader(stream, this.Delimiter);
            this.ResetStream(stream);
        }

        public Delimiter Delimiter { get; private set; }
        public DelimitedHeader Header { get; private set; }

        private void ResetStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        private Delimiter ElectDelimiter(Stream stream)
        {
            var elector = new DelimiterElector(DelimitedParserElector.MaxRowsToRead);
            return elector.Elect(stream);
        }
        
        private DelimitedHeader ElectHeader(Stream stream, Delimiter delimiter)
        {
            var factory = new SpreadsheetFactory(delimiter);
            var spreadsheet = factory.Create(stream, DelimitedParserElector.MaxRowsToRead);

            var elector = new DelimitedHeaderElector();
            return elector.Elect(spreadsheet);
        }
    }
}
