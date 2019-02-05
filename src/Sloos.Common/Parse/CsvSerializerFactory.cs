// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Linq;

namespace Sloos.Common.Parse
{
    public class CsvSerializerFactory
    {
        private readonly DelimiterElector delimiterElector;
        private readonly DelimitedHeaderElector delimitedHeaderElector;

        public CsvSerializerFactory()
        {
            this.delimiterElector = new DelimiterElector(5);
            this.delimitedHeaderElector = new DelimitedHeaderElector();
        }

        public CsvSerializerState Create(string assemblyName, string entityName, Stream toStream)
        {
            var delimiter = this.delimiterElector.Elect(toStream);
            toStream.Seek(0, SeekOrigin.Begin);

            var factory = new SpreadsheetFactory(delimiter);
            var spreadsheet = factory.Create(toStream, 5);
            toStream.Seek(0, SeekOrigin.Begin);

            var delimitedHeader = this.delimitedHeaderElector.Elect(spreadsheet);

            var typeBuilder = new AnonymousEntityTypeBuilder();
            var typeColumns = delimitedHeader
                .DelimitedColumns
                .Select(x => new AnonymousEntityTypeProperty { Name = x.Name, Type = x.Type })
                .ToArray();

            var entityType = typeBuilder.Create(assemblyName, entityName, typeColumns);

            var serializerType = typeof(CsvSerializer<>).MakeGenericType(entityType);

            return CsvSerializerState.Create(
                delimiter,
                delimitedHeader.DelimitedColumns,
                entityType,
                Activator.CreateInstance(serializerType));
        }
    }
}
