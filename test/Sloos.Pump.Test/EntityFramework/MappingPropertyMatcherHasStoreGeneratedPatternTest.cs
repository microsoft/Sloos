// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyMatcherHasStoreGeneratedPatternTest
    {
        private readonly EntityTypeBuilder builder = new EntityTypeBuilder();

        [Fact]
        public void NumericKeyShouldNotMatch()
        {
            var entityType = this.builder
                .Name("My.Table")
                .WithProperty<int>("ID", x => x.StoreGeneratedPattern = StoreGeneratedPattern.Identity)
                .WithKeys("ID")
                .Build();

            var mappingProperty = new MappingProperty(entityType.Properties.Single());

            var testSubject = new MappingPropertyMatcherHasStoreGeneratedPattern(entityType);
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }

        [Fact]
        public void UngeneratedKeyShouldHaveStoreGeneratedNone()
        {
            var entityType = this.builder
                .Name("My.Table")
                .WithProperty<int>("ID", x => x.StoreGeneratedPattern = StoreGeneratedPattern.None)
                .WithKeys("ID")
                .Build();

            var mappingProperty = new MappingProperty(entityType.Properties.Single());

            var testSubject = new MappingPropertyMatcherHasStoreGeneratedPattern(entityType);
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
            testSubject.GetStoreGeneratedPattern(mappingProperty).Should().Be(StoreGeneratedPattern.None);
        }

        [Fact]
        public void CompositeKeyShouldHaveStoreGeneratedNone()
        {
            var entityType = this.builder
                .Name("My.Table")
                .WithProperty<int>("ID", x => x.StoreGeneratedPattern = StoreGeneratedPattern.None)
                .WithProperty<string>("Name", x => x.StoreGeneratedPattern = StoreGeneratedPattern.None)
                .WithKeys("ID", "Name")
                .Build();

            var testSubject = new MappingPropertyMatcherHasStoreGeneratedPattern(entityType);

            var mappingProperty1 = new MappingProperty(entityType.Properties.First(x => x.Name == "ID"));
            testSubject.IsMatch(mappingProperty1).Should().BeTrue();
            testSubject.GetStoreGeneratedPattern(mappingProperty1).Should().Be(StoreGeneratedPattern.None);

            var mappingProperty2 = new MappingProperty(entityType.Properties.First(x => x.Name == "Name"));
            testSubject.IsMatch(mappingProperty2).Should().BeFalse();
        }

        [Fact]
        public void NonKeyMemberWithIdentityShouldHaveStoreGeneratedIdentity()
        {
            var entityType = this.builder
                .Name("My.Table")
                .WithProperty<int>("ID", x => x.StoreGeneratedPattern = StoreGeneratedPattern.Identity)
                .Build();

            var mappingProperty = new MappingProperty(entityType.Properties.Single());

            var testSubject = new MappingPropertyMatcherHasStoreGeneratedPattern(entityType);
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
            testSubject.GetStoreGeneratedPattern(mappingProperty).Should().Be(StoreGeneratedPattern.Identity);
        }

        [Fact]
        public void ConceptualVsStore()
        {
            var factory = new SampleGeneratorFactory(
                CodeFirstGen.OneToMany.Csdl(),
                CodeFirstGen.OneToMany.Ssdl(),
                CodeFirstGen.OneToMany.Msdl());

            // -- STORE ------------------------------
            var storeEntityType = factory.StoreItemCollection.OfType<EntityType>().First(x => x.Name == "Tags");
            var storeMappingProperty = new MappingProperty(storeEntityType.Properties.First(x => x.Name == "ID"));

            var storeTestSubject = new MappingPropertyMatcherHasStoreGeneratedPattern(storeEntityType);
            storeTestSubject.IsMatch(storeMappingProperty).Should().BeFalse();

            // -- CONCEPTUAL ------------------------------
            var conceptualEntityType = factory.ConceptualItemCollection.OfType<EntityType>().First(x => x.Name == "Tag");
            var conceptualMappingProperty = new MappingProperty(conceptualEntityType.Properties.First(x => x.Name == "ID"));

            var conceptualTestSubject = new MappingPropertyMatcherHasStoreGeneratedPattern(conceptualEntityType);
            conceptualTestSubject.IsMatch(conceptualMappingProperty).Should().BeTrue();
            conceptualTestSubject.GetStoreGeneratedPattern(conceptualMappingProperty).Should().Be(StoreGeneratedPattern.None);

            // NOTE: there's a difference!  Make sure it is crystal clear!!
            storeTestSubject.IsMatch(storeMappingProperty).Should().BeFalse();
            conceptualTestSubject.IsMatch(conceptualMappingProperty).Should().BeTrue();
        }
    }
}
