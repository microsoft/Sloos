// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        [Fact]
        public void ArgsParserNegate_NoPrefixNegatesOption_IsCorrect()
        {
            var opts = ArgsParser<MyNegateOption>.Parse(new[] { "--no-flag" });
            Assert.False(opts.Flag);
        }

        private sealed class MyNegateOption
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(DefaultValue="true")] public bool Flag { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
