// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class ContextViewFactoryTest
    {
        [Fact]
        public void Test()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
               CodeFirstGen.Simple.Csdl(),
               CodeFirstGen.Simple.Ssdl(),
               CodeFirstGen.Simple.Msdl());

            var entityTypes = sampleGeneratorFactory.StoreItemCollection.OfType<EntityType>().ToArray();

            var factory = new ContextViewFactory(
                "Context",
                new EntityNamespace("Fact"),
                entityTypes);

            var view = factory.Create();
            view.Name.Should().Be("Context");
            view.ModelNamespace.Should().Be("Fact.Models");
            view.ContextEntityViews.Should().HaveCount(1);
        }
    }
}
