// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Slam.Cosmos.Pump.EntityFramework;
using Xunit;

namespace Slam.Cosmos.Pump.Test.EntityFramework
{
    public class KeyFormatterTest
    {
        [Fact]
        public void OneKeyShouldFormatAsScalar()
        {
            var formatter = new KeyFormatter();
            formatter.Format("key1").Should().Be("x => x.key1");
        }

        [Fact]
        public void MultipleKeysShouldFormatAsAnonymousType()
        {
            var formatter = new KeyFormatter();
            formatter.Format("key1", "key2").Should().Be("x => new { x.key1, x.key2 }");
        }
    }
}
