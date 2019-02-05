// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Sloos.Common
{
    public sealed class SpreadsheetColumnTypeDetector
    {
        private readonly TypeElector typeElector;

        public SpreadsheetColumnTypeDetector()
        {
            this.typeElector = new TypeElector();
        }

        public IEnumerable<ElectedColumnType> Elect(Spreadsheet spreadsheet)
        {
            for (int i = 0; i < spreadsheet.Columns; i++)
            {
                var electedColumnType = new ElectedColumnType()
                {
                    TypeOfAllRows = this.GetTypeOfAllRows(spreadsheet, i),
                    TypeOfNonHeaderRows = this.GetTypeOfNonHeaderRows(spreadsheet, i),
                };

                yield return electedColumnType;
            }
        }

        private Type GetTypeOfNonHeaderRows(Spreadsheet spreadsheet, int i)
        {
            return spreadsheet.ColumnApply(this.typeElector.Elect, i, 1);
        }

        private Type GetTypeOfAllRows(Spreadsheet spreadsheet, int i)
        {
            return spreadsheet.ColumnApply(this.typeElector.Elect, i);
        }
    }
}
