// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Sloos.Common
{
    public sealed class DelimitedRecordParser : IDelimitedParser
    {
        private readonly List<string> fields;
        private readonly CharReader reader;
        private DelimitedParserStateContext context;

        public DelimitedRecordParser(CharReader reader, char delimiter)
        {
            this.reader = reader;
            this.Delimiter = delimiter;

            this.context = new DelimitedParserStateCapture(this);
            this.fields = new List<string>();
        }

        public char Delimiter { get; }

        public void Append(string field)
        {
            this.fields.Add(field);
        }

        public void ChangeState(DelimitedParserStateContext toContext)
        {
            this.context = toContext;
        }

        public IEnumerable<string> Parse()
        {
            foreach (var c in this.reader)
            {
                if (!this.context.Push(c))
                {
                    return this.fields;
                }
            }

            this.context.Complete();
            return this.fields;
        }
    }
}
