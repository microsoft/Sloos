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
    public class MappingRelationshipForeignKeyMatcherTest
    {
        private readonly NavigationPropertyBuilder builder;
        private readonly EntityTypeBuilder entityTypeBuilder;

        private readonly EntityType posts;

        private readonly EntityType tags;

        public MappingRelationshipForeignKeyMatcherTest()
        {
            this.builder = new NavigationPropertyBuilder();
            this.entityTypeBuilder = new EntityTypeBuilder();

            this.tags = this.entityTypeBuilder
                .Name("My.Tags")
                .WithProperty<int>("ID")
                .WithProperty<int>("Post_ID")
                .WithKeys("ID")
                .Build();

            this.posts = this.entityTypeBuilder
                .Name("My.Posts")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();        
        }

        [Fact(Skip = "Debug only.")]
        [Trait("IsInteractive","true")]
        public void BlahBlah()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.OneToMany.Csdl(),
                CodeFirstGen.OneToMany.Ssdl(),
                CodeFirstGen.OneToMany.Msdl());

            var aa = sampleGeneratorFactory.StoreItemCollection.Last();
            var ab = sampleGeneratorFactory.ConceptualItemCollection.Last();

            var edmMapping = new EdmMapping(
                sampleGeneratorFactory.ConceptualItemCollection,
                sampleGeneratorFactory.StoreItemCollection,
                CodeFirstGen.OneToMany.Msdl());
        }

        [Fact]
        public void HasRequiredNecessityShouldBeHasReuired()
        {
            var testSubject = new MappingRelationshipForeignKeyMatcherHasRequired(
                new CodeGenEscaper());

            testSubject.GetNecessity().Should().Be("HasRequired");
        }

        [Fact]
        public void HasRequiredShouldBeFalse()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.Many)
                .ToRole(this.tags, RelationshipMultiplicity.ZeroOrOne)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasRequired(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeFalse();
        }

        [Fact]
        public void HasRequiredShouldBeTrue()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.Many)
                .ToRole(this.tags, RelationshipMultiplicity.One)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasRequired(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeTrue();
            testSubject.GetEntityName(navigationProperty).Should().Be("Tags");
        }

        [Fact]
        public void HasOptionalNecessityShouldBeHasOptional()
        {
            var testSubject = new MappingRelationshipForeignKeyMatcherHasOptional(
                new CodeGenEscaper());

            testSubject.GetNecessity().Should().Be("HasOptional");
        }

        [Fact]
        public void HasOptionalShouldBeFalse()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.Many)
                .ToRole(this.tags, RelationshipMultiplicity.One)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasOptional(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeFalse();
        }

        [Fact]
        public void HasOptionalShouldBeTrue()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.Many)
                .ToRole(this.tags, RelationshipMultiplicity.ZeroOrOne)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasOptional(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeTrue();
            testSubject.GetEntityName(navigationProperty).Should().Be("Tags");
        }

        [Fact]
        public void WithOptionalShouldBeTrue()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.One)
                .ToRole(this.tags, RelationshipMultiplicity.Many)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherWithOptional(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeTrue();
            testSubject.GetEntityName(navigationProperty).Should().Be("Tags");
        }

        [Fact]
        public void WithOptionalShouldBeFalse()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.Many)
                .ToRole(this.tags, RelationshipMultiplicity.One)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherWithOptional(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeFalse();
        }

        [Fact]
        public void WithManyShouldBeTrue()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.Many)
                .ToRole(this.tags, RelationshipMultiplicity.One)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherWithMany(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeTrue();
            testSubject.GetEntityName(navigationProperty).Should().Be("Tags");
        }

        [Fact]
        public void WithManyShouldBeFalse()
        {
            var navigationProperty = this.builder
                .FromRole(this.posts, RelationshipMultiplicity.One)
                .ToRole(this.tags, RelationshipMultiplicity.Many)
                .Map("ID", "Post_ID")
                .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherWithMany(
                new CodeGenEscaper());
            testSubject.IsMatch(navigationProperty).Should().BeFalse();
        }

        [Fact]
        public void HasOneForeignKeyShouldBeTrue()
        {
            var navigationProperty = this.builder
               .FromRole(this.posts, RelationshipMultiplicity.Many)
               .ToRole(this.tags, RelationshipMultiplicity.One)
               .Map("ID", "Post_ID")
               .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasForeignKey();
            testSubject.IsMatch(navigationProperty).Should().BeTrue();

            testSubject.ForeignKeys.Should().HaveCount(1);
            testSubject.ForeignKeys.First().Should().Be("Post_ID");
        }

        [Fact]
        public void HasManyForeignKeysShouldBeTrue()
        {
            var multiKeyTags = this.entityTypeBuilder
              .Name("My.Tags")
              .WithProperty<int>("ID")
              .WithProperty<int>("Post_00_ID")
              .WithProperty<int>("Post_01_ID")
              .WithKeys("ID")
              .Build();

            var navigationProperty = this.builder
              .FromRole(this.posts, RelationshipMultiplicity.Many)
              .ToRole(multiKeyTags, RelationshipMultiplicity.One)
              .Map(new[] {"ID"}, new[] { "Post_00_ID", "Post_01_ID"  })
              .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasForeignKey();
            testSubject.IsMatch(navigationProperty).Should().BeTrue();

            testSubject.ForeignKeys.Should().HaveCount(2);
            var foreignKeys = testSubject.ForeignKeys.ToArray();
            foreignKeys[0].Should().Be("Post_00_ID");
            foreignKeys[1].Should().Be("Post_01_ID");
        }

        [Fact]
        public void HasForeignKeyShouldBeFalse()
        {
            var navigationProperty = this.builder
               .FromRole(this.posts, RelationshipMultiplicity.One)
               .ToRole(this.tags, RelationshipMultiplicity.Many)
               .Map("ID", "Post_ID")
               .Build();

            var testSubject = new MappingRelationshipForeignKeyMatcherHasForeignKey();
            testSubject.IsMatch(navigationProperty).Should().BeFalse();
            testSubject.ForeignKeys.Should().HaveCount(0);
        }
    }
}
