// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sloos.Common.Test
{
    public partial class ArgsParserTest
    {
        [Fact]
        public void ArgsParserUnparsedArgs_ArgsAtEnd_AreCollected()
        {
            var opts1 = ArgsParser<MyUnparsedOption1>.Parse(new[] { "-f", "dog", "cat" });
            Assert.True(opts1.Flag);

            Assert.Equal(2, opts1.Args.Length);
            Assert.Equal("dog", opts1.Args[0]);
            Assert.Equal("cat", opts1.Args[1]);
        }

        [Fact]
        public void ArgsParserUnparsedArgs_ArgsOfTypeListString_IsSet()
        {
            var opts1 = ArgsParser<MyUnparsedOption2<List<string>>>.Parse(new[] { "dog", "cat" });

            Assert.Equal(2, opts1.Args.Count);
            Assert.Equal("dog", opts1.Args.ElementAt(0));
            Assert.Equal("cat", opts1.Args.ElementAt(1));
        }

        [Fact]
        public void ArgsParserUnparsedArgs_ArgsOfTypeQueueString_IsSet()
        {
            var opts1 = ArgsParser<MyUnparsedOption2<Queue<string>>>.Parse(new[] { "dog", "cat" });

            Assert.Equal(2, opts1.Args.Count);
            Assert.Equal("dog", opts1.Args.ElementAt(0));
            Assert.Equal("cat", opts1.Args.ElementAt(1));
        }

        [Fact]
        public void ArgsParserUnparsedArgs_ArgsOfTypeHashSetString_IsSet()
        {
            var opts1 = ArgsParser<MyUnparsedOption2<HashSet<string>>>.Parse(new[] { "dog", "cat" });

            Assert.Equal(2, opts1.Args.Count);
            Assert.Equal("dog", opts1.Args.ElementAt(0));
            Assert.Equal("cat", opts1.Args.ElementAt(1));
        }

        [Fact]
        public void ArgsParserUnparsedArgs_UnparsedInTheMiddle_IsSet()
        {
            var opts1 = ArgsParser<MyUnparsedOption1>.Parse(new[] { "dog", "-f", "cat" });

            Assert.Equal(2, opts1.Args.Length);
            Assert.True(opts1.Flag);
            Assert.Equal("dog", opts1.Args.ElementAt(0));
            Assert.Equal("cat", opts1.Args.ElementAt(1));
        }

        private sealed class MyUnparsedOption1
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [Option(ShortName="f")] public bool Flag { get; set; }
            [UnparsedArguments] public string[] Args { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private sealed class MyUnparsedOption2<T>
            where T : IEnumerable<string>
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            [UnparsedArguments] public T Args { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
