// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sloos.Common
{
    public sealed class Spreadsheet
    {
        private readonly string[][] grid;

        public Spreadsheet(string[][] grid)
        {
            this.grid = grid;
        }

        public int Rows => this.grid.Length;
        public int Columns => this.grid[0].Length;

        public T ColumnApply<T>(Func<string[], T> func, int column, int rowsToSkip)
        {
            return func(
                this.grid.Skip(rowsToSkip).Select(x => x[column]).ToArray());
        }

        public T ColumnApply<T>(Func<string[], T> func, int column)
        {
            return this.ColumnApply(func, column, 0);
        }

        public IEnumerable<string> GetRow(int row)
        {
            return this.grid[row];
        }
    }
}
