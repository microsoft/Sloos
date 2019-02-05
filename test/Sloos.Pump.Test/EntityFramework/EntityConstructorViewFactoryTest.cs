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
    public class EntityConstructorViewFactoryTest
    {
        [Fact]
        public void EntityConstructorViewFactoryShouldCreateOnlyForManyMultiplicity()
        {
            var sampleFactoryGenerator = new SampleGeneratorFactory(
                CodeFirstGen.OneToMany.Csdl(),
                CodeFirstGen.OneToMany.Ssdl(),
                CodeFirstGen.OneToMany.Msdl());

            var postConceptualEntityType = sampleFactoryGenerator.ConceptualItemCollection
                .OfType<EntityType>()
                .Single(x => x.Name == "Post");

            var testSubject = new EntityConstructorViewFactory(new CodeGenEscaper());

            var views = testSubject.Create(postConceptualEntityType).ToArray();
            views.Should().HaveCount(1);
            views[0].Name.Should().Be("Tags");
            views[0].TypeName.Should().Be("global::System.Collections.Generic.List<Tag>");
        }

        [Fact]
        public void EntityConstructorViewFactoryShouldNotCreateOnlyForNonManyMultiplicity()
        {
            var sampleFactoryGenerator = new SampleGeneratorFactory(
                CodeFirstGen.OneToMany.Csdl(),
                CodeFirstGen.OneToMany.Ssdl(),
                CodeFirstGen.OneToMany.Msdl());

            var postConceptualEntityType = sampleFactoryGenerator.ConceptualItemCollection
                .OfType<EntityType>()
                .Single(x => x.Name == "Tag");

            var testSubject = new EntityConstructorViewFactory(new CodeGenEscaper());

            var views = testSubject.Create(postConceptualEntityType).ToArray();
            views.Should().BeEmpty();
        }
    }
}
