// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sloos.Common.Test
{
    class DelimitedParserStub : IDelimitedParser
    {
        private DelimitedParserStub(char delimiter)
        {
            this.Delimiter = delimiter;
            this.Fields = new List<string>();
        }

        public List<string> Fields { get; }

        public DelimitedParserStateContext Context { get; set; }

        public char Delimiter { get; }

        public void Append(string field)
        {
            this.Fields.Add(field);
        }

        public void ChangeState(DelimitedParserStateContext context)
        {
            this.Context = context;
        }

        public IEnumerable<string> Parse(string s)
        {
            foreach (var c in s.ToCharArray())
            {
                this.Context.Push(c);
            }

            this.Context.Complete();
            return this.Fields;
        }

        public static DelimitedParserStub Create<T>(char delimiter, params object[] args)
            where T : DelimitedParserStateContext
        {
            var parser = new DelimitedParserStub(delimiter);
            var constructorArgs = Enumerable.Repeat<object>(parser, 1).Concat(args);

            parser.Context = (T)Activator.CreateInstance(typeof(T), constructorArgs.ToArray());

            return parser;
        }
    }
}
