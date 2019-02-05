// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;

namespace Sloos.Pump.Test
{
    public class SqlCompactContextFactory
    {
        private readonly string connectionString;

        public SqlCompactContextFactory()
            : this(Path.GetRandomFileName())
        {
        }

        public SqlCompactContextFactory(string filename)
        {
            this.connectionString = $"Data Source={filename};";
        }

        public T Create<T>(Func<string, T> factory)
        {
            return factory(this.connectionString);
        }
    }
}
