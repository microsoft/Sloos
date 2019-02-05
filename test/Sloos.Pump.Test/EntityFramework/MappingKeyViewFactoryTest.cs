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
    public class MappingKeyViewFactoryTest
    {
        [Fact]
        public void OneKeyShouldReturnOneHasKey()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
               CodeFirstGen.Simple.Csdl(),
               CodeFirstGen.Simple.Ssdl(),
               CodeFirstGen.Simple.Msdl());

            var entityType = sampleGeneratorFactory.StoreItemCollection.OfType<EntityType>()
                .Single(x => x.Name == "Tables");

            var testSubject = new MappingKeyViewFactory(entityType);
            
            var mappingKeyView = testSubject.Create();
            mappingKeyView.Keys.Should().HaveCount(1);
            mappingKeyView.Keys.First().Should().Be("ID");
            mappingKeyView.AsStatement().Should().Be("this.HasKey(x => x.ID);");
        }

        [Fact]
        public void CompositeKeyShouldReturnTwoHasKey()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
               CodeFirstGen.CompositeKey.Csdl(),
               CodeFirstGen.CompositeKey.Ssdl(),
               CodeFirstGen.CompositeKey.Msdl());

            var entityType = sampleGeneratorFactory.StoreItemCollection.OfType<EntityType>()
                .Single(x => x.Name == "Tables");

            var testSubject = new MappingKeyViewFactory(entityType);

            var mappingKeyView = testSubject.Create();
            mappingKeyView.Keys.Should().HaveCount(2);
            mappingKeyView.Keys.First().Should().Be("ID");
            mappingKeyView.Keys.Last().Should().Be("Name");
            mappingKeyView.AsStatement().Should().Be("this.HasKey(x => new { x.ID, x.Name });");
        }
    }
}
