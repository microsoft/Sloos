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
    public class MappingPropertyViewFactoryTest
    {
        public class Simple
        {
            private readonly MappingPropertyView[] mappingPropertyViews;

            public Simple()
            {
                var sampleFactoryGenerator = new SampleGeneratorFactory(
                    CodeFirstGen.Simple.Csdl(),
                    CodeFirstGen.Simple.Ssdl(),
                    CodeFirstGen.Simple.Msdl());

                var entityType = sampleFactoryGenerator.StoreItemCollection
                    .OfType<EntityType>()
                    .Single(x => x.Name == "Tables");

                var testSubject = new MappingPropertyViewFactory(new CodeGenEscaper());
                this.mappingPropertyViews = testSubject.Create(entityType).ToArray();
            }

            [Fact]
            public void PropertyNames()
            {
                this.mappingPropertyViews.Should().HaveCount(2);
                this.mappingPropertyViews
                    .Select(x => x.ConceptualName)
                    .Should()
                    .ContainInOrder("ID", "Name");
            }

            [Fact]
            public void PropertiesShouldHaveNoAttributes()
            {
                this.mappingPropertyViews
                    .First(x => x.ConceptualName == "ID")
                    .Attributes.Should().BeEmpty();

                this.mappingPropertyViews
                    .First(x => x.ConceptualName == "Name")
                    .Attributes.Should().BeEmpty();
            }
        }

        public class CompositeKey
        {
            private readonly MappingPropertyView[] mappingPropertyViews;

            public CompositeKey()
            {
                var sampleFactoryGenerator = new SampleGeneratorFactory(
                    CodeFirstGen.CompositeKey.Csdl(),
                    CodeFirstGen.CompositeKey.Ssdl(),
                    CodeFirstGen.CompositeKey.Msdl());

                var entityType = sampleFactoryGenerator.StoreItemCollection
                    .OfType<EntityType>()
                    .Single(x => x.Name == "Tables");

                var testSubject = new MappingPropertyViewFactory(new CodeGenEscaper());
                this.mappingPropertyViews = testSubject.Create(entityType).ToArray();
            }

            [Fact]
            public void ID()
            {
                var property = this.mappingPropertyViews.Single(x => x.ConceptualName == "ID");
                property.Attributes
                    .Should()
                    .Contain(new[] { ".HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)" });
            }
        }

        public class DateTime
        {
            private readonly MappingPropertyView[] mappingPropertyViews;

            public DateTime()
            {
                var sampleFactoryGenerator = new SampleGeneratorFactory(
                    CodeFirstGen.DateTime.Csdl(),
                    CodeFirstGen.DateTime.Ssdl(),
                    CodeFirstGen.DateTime.Msdl());

                var entityType = sampleFactoryGenerator.StoreItemCollection
                    .OfType<EntityType>()
                    .Single(x => x.Name == "Tables");

                var testSubject = new MappingPropertyViewFactory(new CodeGenEscaper());
                this.mappingPropertyViews = testSubject.Create(entityType).ToArray();
            }

            [Fact]
            public void TimeStamp()
            {
                var property = this.mappingPropertyViews.Single(x => x.ConceptualName == "TimeStamp");
                property.Attributes
                    .Should()
                    .ContainInOrder(new[] { ".IsRequired()", ".IsFixedLength()", ".HasMaxLength(8)", ".IsRowVersion()" });
            }
        }

        public class Strings 
        { 
            private readonly MappingPropertyView[] mappingPropertyViews;

            public Strings()
            {
                var sampleFactoryGenerator = new SampleGeneratorFactory(
                    CodeFirstGen.Strings.Csdl(),
                    CodeFirstGen.Strings.Ssdl(),
                    CodeFirstGen.Strings.Msdl());

                var entityType = sampleFactoryGenerator.StoreItemCollection
                    .OfType<EntityType>()
                    .Single(x => x.Name == "Tables");

                var testSubject = new MappingPropertyViewFactory(new CodeGenEscaper());
                this.mappingPropertyViews = testSubject.Create(entityType).ToArray();
            }

            [Fact]
            public void Required()
            {
                var property = this.mappingPropertyViews.Single(x => x.ConceptualName == "Required");
                property.Attributes
                    .Should()
                    .Contain(new[] { ".IsRequired()" });
            }

            [Fact]
            public void MaxLength128()
            {
                var property = this.mappingPropertyViews.Single(x => x.ConceptualName == "MaxLength128");
                property.Attributes
                    .Should()
                    .Contain(new[] { ".HasMaxLength(128)" });
            }

            [Fact]
            public void MinLength100MaxLength128()
            {
                var property = this.mappingPropertyViews.Single(x => x.ConceptualName == "MinLength100MaxLength128");
                property.Attributes
                    .Should()
                    .Contain(new[] { ".HasMaxLength(128)" });
            }

            [Fact]
            public void FixedLength()
            {
                var property = this.mappingPropertyViews.Single(x => x.ConceptualName == "FixedLength");
                property.Attributes
                    .Should()
                    .ContainInOrder(new[] { ".IsFixedLength()", ".HasMaxLength(100)" });
            }
        }
    }
}
