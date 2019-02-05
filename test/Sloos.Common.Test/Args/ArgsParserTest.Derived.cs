// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        [Fact]
        public void ArgsParserDerived_ParentOptions_IsCorrect()
        {
            var opts = ArgsParser<ChildOption1>.Parse(new[] { "--flag", "-h", "--value=1" });
            Assert.True(opts.Flag);
            Assert.True(opts.Help);
            Assert.Equal(1, opts.Value);
        }

        [Fact]
        public void ArgsParserDerived_ConnectionString()
        {
            var opts = ArgsParser<ConnectionStringOption>.Parse(new[] { "--flag", "--connection-string", @"""Server=localhost;Database=My;Trusted_Connection=True;""" });
            Assert.True(opts.Flag);
            Assert.Equal("Server=localhost;Database=My;Trusted_Connection=True;", opts.ConnectionString);
        }

        private sealed class ConnectionStringOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option] public bool Flag { get; set; }
            [Option] public string ConnectionString { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class ChildOption1 : ParentOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option] public int Value { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private class ParentOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option] public bool Flag { get; set; }
            [Option(ShortName="h")] public bool Help { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
