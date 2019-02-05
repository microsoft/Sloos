// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace Sloos.Common
{
    public sealed class DelimitedHeaderElector 
    {
        public DelimitedHeader Elect(Spreadsheet spreadsheet)
        {
            var spreadsheetElector = new SpreadsheetColumnTypeDetector();
            var columns = spreadsheetElector.Elect(spreadsheet)
                .ToArray();

            bool hasHeader = this.HasHeader(columns);
            var columnNames = hasHeader ?
                spreadsheet.GetRow(0) :
                this.GetDefaultColumnNames(spreadsheet.Columns);

            var columnTypes = hasHeader ?
                columns.Select(x => x.TypeOfNonHeaderRows) :
                columns.Select(x => x.TypeOfAllRows);

            var delimitedColumns = columnNames
                .Zip(columnTypes, (name, type) => new DelimitedColumn { Name = name, Type = type });

            var delimitedHeader = new DelimitedHeader(
                delimitedColumns);

            return delimitedHeader;
        }

        private bool HasHeader(IEnumerable<ElectedColumnType> columns)
        {
            return columns.Any(x => x.TypeOfAllRows != x.TypeOfNonHeaderRows);
        }

        private IEnumerable<string> GetDefaultColumnNames(int countOfColumns)
        {
            for (int i=0; i < countOfColumns; i++)
            {
                yield return $"Column{i}";
            }
        }
    }
}
