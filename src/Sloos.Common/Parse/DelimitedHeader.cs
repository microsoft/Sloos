// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sloos.Common
{
    public sealed class DelimitedHeader
    {
        public DelimitedHeader(IEnumerable<DelimitedColumn> columnInfos)
        {
            this.DelimitedColumns = columnInfos.ToArray();
        }

        public int Count => this.DelimitedColumns.Length;

        public string this[int index] => this.DelimitedColumns[index].Name;
        public int this[string name] => this.GetOrdinal(name);

        public DelimitedColumn[] DelimitedColumns { get; }

        public int GetOrdinal(string name)
        {
            return Array.FindIndex(this.DelimitedColumns, x => x.Name == name);
        }
    }
}
