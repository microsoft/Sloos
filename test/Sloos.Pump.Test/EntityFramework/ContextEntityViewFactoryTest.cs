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
    public class ContextEntityViewFactoryTest
    {
        [Fact]
        public void Test()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.Simple.Csdl(),
                CodeFirstGen.Simple.Ssdl(),
                CodeFirstGen.Simple.Msdl());

            var entityType = sampleGeneratorFactory.StoreItemCollection.OfType<EntityType>()
                .Single(x => x.Name == "Tables");

            var testSubject = new ContextEntityViewFactory();

            var view = testSubject.Create(entityType);
            view.ConceptualName.Should().Be("Table");
            view.ConceptualMappingName.Should().Be("TableMap");
            view.StoreName.Should().Be("Tables");
        }
    }
}
