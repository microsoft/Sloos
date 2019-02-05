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
    public class MappingColumnNameViewFactoryTest
    {
        [Fact]
        public void ColumnNameShouldBeOrdered()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
               CodeFirstGen.Simple.Csdl(),
               CodeFirstGen.Simple.Ssdl(),
               CodeFirstGen.Simple.Msdl());

            var edmMapping = sampleGeneratorFactory.EdmMapping;

            var entityType = sampleGeneratorFactory.ConceptualItemCollection.OfType<EntityType>()
                .Single(x => x.Name == "Table");

            var testSubject = new MappingColumnNameViewFactory(edmMapping);
            var mappingColumnNameViews = testSubject.Create(entityType).ToArray();
            
            mappingColumnNameViews.Should().HaveCount(2);
            mappingColumnNameViews.Select(x => x.ConceptualName).Should().ContainInOrder("ID", "Name");
            mappingColumnNameViews.Select(x => x.StoreName).Should().ContainInOrder("ID", "Name");
        }
    }
}
