// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        [Fact]
        public void ArgsParserDelimitedValue_LongName_IsSet()
        {
            var opts1 = ArgsParser<MyDelimitedOption1>.Parse(new[] { "--int=1" });
            Assert.Equal(1, opts1.Int);
        }

        [Fact]
        public void ArgsParserDelimitedValue_ShortName_IsSet()
        {
            var opts1 = ArgsParser<MyDelimitedOption1>.Parse(new[] { "-i", "1" });
            Assert.Equal(1, opts1.Int);
        }
     
        private sealed class MyDelimitedOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(ShortName = "i")]
            public int Int { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
