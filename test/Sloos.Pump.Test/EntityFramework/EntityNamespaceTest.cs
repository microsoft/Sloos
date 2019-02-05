// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Sloos.Pump.EntityFramework;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class EntityNamespaceTest
    {
        [Fact]
        public void Test()
        {
            var ns = new EntityNamespace("My");

            Assert.Equal("My", ns.Namespace);
            Assert.Equal("My.Models.Mapping", ns.MappingNamespace);
            Assert.Equal("My.Models", ns.ModelNamespace);
        }
    }
}
