// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using Sloos.Common;
using Xunit;

namespace Sloos.Test
{
    public class StringSplitTest
    {
        [Fact]
        public void AtIndex()
        {
            string s = "abc,1,hello,,";

            Assert.Equal("abc", StringSplit.AtIndex(s, ",", 0));
            Assert.Equal("1", StringSplit.AtIndex(s, ",", 1));
            Assert.Equal("hello", StringSplit.AtIndex(s, ",", 2));
            Assert.Equal("", StringSplit.AtIndex(s, ",", 3));
        }

        [Fact]
        public void AtIndex_ExceedsBoundary_ThrowsException()
        {
            string s = "abc,1,hello,,";

            try
            {
                StringSplit.AtIndex(s, ",", 5);
                Assert.True(false, "Did not expect to execute this line!");
            }
            catch (ArgumentOutOfRangeException)
            {
                // pass
            }
        }

        [Fact]
        public void AtIndex_NonMatchingSeparator_ReturnsOriginalString()
        {
            string s = "abc,1,hello,,";
            Assert.Equal(s, StringSplit.AtIndex(s, "|", 0));
        }

        [Fact]
        public void AtIndex_MultiCharacterSplit()
        {
            string s = "123abc456";
            Assert.Equal("123", StringSplit.AtIndex(s, "abc", 0));
            Assert.Equal("456", StringSplit.AtIndex(s, "abc", 1));
        }

    }
}
