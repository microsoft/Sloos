// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;

namespace Sloos.Common.Parse
{
    public class CsvSerializerState
    {
        private CsvSerializerState(Delimiter delimiter, DelimitedColumn[] delimitedColumn, Type entityType, object serializer)
        {
            this.Delimiter = delimiter.DelimitedBy;
            this.Names = delimitedColumn.Select(x => x.Name).ToArray();
            this.Types = delimitedColumn.Select(x => x.Type).ToArray();
            this.EntityType = entityType;
            this.Serializer = serializer;
        }

        public char Delimiter { get; set; }
        public string[] Names { get; set; }
        public Type[] Types { get; set; }
        public Type EntityType { get; }
        public object Serializer { get; set; }

        public static CsvSerializerState Create(Delimiter delimiter, DelimitedColumn[] delimitedColumns, Type entityType, object serializer)
        {
            return new CsvSerializerState(delimiter, delimitedColumns, entityType, serializer);
        }
    }
}
