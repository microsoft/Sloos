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
    public class MappingRelationshipForeignKeyViewFactoryTest
    {
        private readonly MappingRelationshipForeignKeyView[] foreignKeyViews;

        public MappingRelationshipForeignKeyViewFactoryTest()
        {
            var sampleFactoryGenerator = new SampleGeneratorFactory(
                    CodeFirstGen.OneToMany.Csdl(),
                    CodeFirstGen.OneToMany.Ssdl(),
                    CodeFirstGen.OneToMany.Msdl());

            var entityType = sampleFactoryGenerator.ConceptualItemCollection
                .OfType<EntityType>()
                .Single(x => x.Name == "Tag");

            var testSubject = new MappingRelationshipForeignKeyViewFactory(new CodeGenEscaper());
            this.foreignKeyViews = testSubject.Create(entityType).ToArray();
        }

        [Fact]
        public void Test()
        {
            var view = this.foreignKeyViews[0];

            view.EntityNecessity.Should().Be("HasOptional");
            view.EntityName.Should().Be("Post");

            var attributes = view.Attributes.ToArray();
            attributes.Should().HaveCount(2);
            attributes[0].Should().Be(".WithMany(x => x.Tags)");
            attributes[1].Should().Be(".HasForeignKey(x => x.Post_ID)");
        }
    }
}
