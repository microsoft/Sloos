// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos
{
    public class Options
    {
        [Option(ShortName="t", LongName="table", Description="name of the destination table")]
        public string Table { get; set; }

        [Option(ShortName="s", LongName="source", Description="source of the data, e.g. a file name")]
        public string Source { get; set; }

        [Option(ShortName="d", LongName="destination", Description="destination of data expressed as a connection string")]
        public string Destination { get; set; }

        [Option(DefaultValue = "5000", ShortName="b", LongName="batch-size", Description="number of rows in each batch")]
        public int BatchSize { get; set; }
    }
}
