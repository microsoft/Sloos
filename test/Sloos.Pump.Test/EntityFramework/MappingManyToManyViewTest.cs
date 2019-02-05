// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingManyToManyViewTest
    {
        //private readonly NavigationPropertyBuilder builder;

        //private readonly EntityType posts;
        //private readonly EntityType tags;
        //private readonly EntityType tagsToPosts;

        //public MappingManyToManyTest()
        //{
        //    this.builder = new NavigationPropertyBuilder();

        //    var entityTypeBuilder = new EntityTypeBuilder();
        //    this.posts = entityTypeBuilder
        //        .Name("My.Posts")
        //        .WithProperty<int>("ID")
        //        .WithKeys("ID")
        //        .Build();

        //    this.tags = entityTypeBuilder
        //        .Name("My.Tags")
        //        .WithProperty<int>("ID")
        //        .WithKeys("ID")
        //        .Build();

        //    this.tagsToPosts = entityTypeBuilder
        //        .Name("My.Tags_Posts")
        //        .WithProperty<int>("ID")
        //        .WithProperty<int>("Tag_ID")
        //        .WithProperty<int>("Post_ID")
        //        .WithKeys("ID")
        //        .Build();
        //}

        [Fact]
        public void DebugOnly()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
               CodeFirstGen.ManyToMany.Csdl(),
               CodeFirstGen.ManyToMany.Ssdl(),
               CodeFirstGen.ManyToMany.Msdl());

            var edmMapping = sampleGeneratorFactory.EdmMapping;

            var posts = sampleGeneratorFactory.ConceptualItemCollection.OfType<EntityType>().First(x => x.Name == "Post");

            var entityTypes = sampleGeneratorFactory.ConceptualItemCollection.OfType<EntityType>().ToArray();
            entityTypes.Should().HaveCount(2);

            //var tableSet = edmMapping.EntityMappings[posts].Item1;
            //var propertyToColumnMappings = edmMapping.EntityMappings[posts].Item2;
            
            ///////////////////////////////////////////////////////////////////
            // ManyToManyMappings:
            //  k: AssociationType (aka PostTags)
            //  v: Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>
            //    Item1: EntitySet (aka PostTags - the table in the store)
            //    Item2: Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>
            //      RelationshipEndMember: represents one end of a relationship, in the case of a many
            //                             to many it represents the entity that connects to the
            //                             many-to-many table.
            //      <EdmMember, string>:   PostId, PostId_Post
            //                             represents a mapping to the many-to-many table.  EdmMember
            //                             is the source, and string is the name of the target store
            //                             column.
            //var manyToManyMappings = edmMapping.ManyToManyMappings;

            posts.NavigationProperties.Should().HaveCount(1);
            var manyManyRelationship = posts
                .NavigationProperties
                .Where(x =>
                    x.DeclaringType == posts &&
                    x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many &&
                    x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many &&
                    x.RelationshipType.RelationshipEndMembers.First() == x.FromEndMember);

            NavigationProperty navigationProperty = manyManyRelationship.Single();
            var otherNavigationProperty = navigationProperty
                .ToEndMember
                .GetEntityType()
                .NavigationProperties
                .Single(x => x.RelationshipType == navigationProperty.RelationshipType && x != navigationProperty);

            var association = (AssociationType)navigationProperty.RelationshipType;
            var mapping = edmMapping.ManyToManyMappings[association];
            EntitySet storeEntitySet = mapping.Item1;
            var mappingTableName  = (string)storeEntitySet.MetadataProperties["Table"].Value ?? storeEntitySet.Name;
            var mappingSchemaName = (string)storeEntitySet.MetadataProperties["Schema"].Value;

            EntityType leftType = (EntityType)navigationProperty.DeclaringType;
            leftType.Name.Should().Be("Post");
            Dictionary<EdmMember, string> leftKeyMappings = mapping.Item2[navigationProperty.FromEndMember];
            var leftColumns = string.Join(",", leftType.KeyMembers.Select(x => leftKeyMappings[x]));

            EntityType rightType = (EntityType)otherNavigationProperty.DeclaringType;
            rightType.Name.Should().Be("Tag");
            Dictionary<EdmMember, string> rightKeyMappings = mapping.Item2[otherNavigationProperty.FromEndMember];
            var rightColumns = string.Join(",", rightType.KeyMembers.Select(x => rightKeyMappings[x]));

            // HasMany
            navigationProperty.Name.Should().Be("Tags");

            // WithMany
            otherNavigationProperty.Name.Should().Be("Posts");

            // ToTable
            mappingTableName.Should().Be("PostTags");
            mappingSchemaName.Should().Be("dbo");

            // MapLeftKey
            leftColumns.Should().Be("Post_PostId");

            // MapRightKey
            rightColumns.Should().Be("Tag_TagId");
        }

        [Fact]
        public void StillCrappyTest()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
               CodeFirstGen.ManyToMany.Csdl(),
               CodeFirstGen.ManyToMany.Ssdl(),
               CodeFirstGen.ManyToMany.Msdl());

            var posts = sampleGeneratorFactory.ConceptualItemCollection.OfType<EntityType>().First(x => x.Name == "Post");
            var factory = new MappingManyToManyViewFactory(
                sampleGeneratorFactory.EdmMapping);

            var testSubject = factory.Create(posts).Single();
            testSubject.HasMany.Should().Be("Tags");
            testSubject.WithMany.Should().Be("Posts");
            testSubject.MappingTableName.Should().Be("PostTags");
            testSubject.MappingSchemaName.Should().Be("dbo");
            testSubject.MapLeftKeys.Should().ContainInOrder("Post_PostId");
            testSubject.MapRightKeys.Should().ContainInOrder("Tag_TagId");

            sampleGeneratorFactory.ConceptualItemCollection
                .OfType<EntityType>()
                .SelectMany(factory.Create)
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public void ManyToManyNavigationShouldBeCorrect()
        {
            var entityTypeBuilder = new EntityTypeBuilder();

            var posts = entityTypeBuilder
                .Name("My.Posts")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();

            var tags = entityTypeBuilder
                .Name("My.Tags")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();

            var builder = new NavigationPropertyBuilder();
            var postsToTags = builder
                .FromRole(posts, RelationshipMultiplicity.Many)
                .ToRole(tags, RelationshipMultiplicity.Many)
                .Map("ID", "Post_ID")
                .Build();

            var tagsToPosts = builder
                .FromRole(tags, RelationshipMultiplicity.Many)
                .ToRole(posts, RelationshipMultiplicity.Many)
                .Map("ID", "Tag_ID")
                .WithSameRelationshipAs(postsToTags)
                .Build();

            posts.AddNavigationProperty(postsToTags);
            tags.AddNavigationProperty(tagsToPosts);

            var testSubject = new ManyToManyNavigation(tagsToPosts);
            testSubject.HasMany.Name.Should().Be("Posts");
            testSubject.HasManyEntity.Should().BeSameAs(tags);
            testSubject.HasManyEndMember.Name.Should().Be("Tags");

            testSubject.WithMany.Name.Should().Be("Tags");
            testSubject.WithManyEntity.Should().BeSameAs(posts);
            testSubject.WithManyEndMember.Name.Should().Be("Posts");
        }

        [Fact]
        public void ManyToManyStoreTableShouldBeCorrect()
        {
            var entityTypeBuilder = new EntityTypeBuilder();
            var posts = entityTypeBuilder
                .Name("My.Post")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();

            var entitySet = EntitySet.Create(
                "name",
                "mappingSchemaName",
                "mappingTableName",
                "definingQuery",
                posts,
                Enumerable.Empty<MetadataProperty>());

            var testSubject = new ManyToManyStoreTable(entitySet);
            testSubject.MappingTableName.Should().Be("mappingTableName");
            testSubject.MappingSchemaName.Should().Be("mappingSchemaName");
        }

        [Fact]
        public void NullTableNameShouldDefaultToEntitySetName()
        {
            var entityTypeBuilder = new EntityTypeBuilder();
            var posts = entityTypeBuilder
                .Name("My.Post")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();

            var entitySet = EntitySet.Create(
                "name",
                "mappingSchemaName",
                null,
                "definingQuery",
                posts,
                Enumerable.Empty<MetadataProperty>());

            var testSubject = new ManyToManyStoreTable(entitySet);
            testSubject.MappingTableName.Should().Be("name");
            testSubject.MappingSchemaName.Should().Be("mappingSchemaName");
        }
        
        [Fact]
        public void ManyToManyKeysShouldMapKeys()
        {
            var entityTypeBuilder = new EntityTypeBuilder();
            var posts = entityTypeBuilder
                .Name("My.Post")
                .WithProperty<int>("ID")
                .WithProperty<int>("_ignore0")
                .WithProperty<int>("_ignore1")
                .WithKeys("ID")
                .Build();

            var keyMappings = new Dictionary<EdmMember, string>()
            {
                { posts.Properties.First(), "Post_ID" },
            };

            var testSubject = new ManyToManyKeys(
                posts, keyMappings,
                posts, keyMappings);

            testSubject.MapRightKeys.Should().ContainInOrder("Post_ID");
            testSubject.MapLeftKeys.Should().ContainInOrder("Post_ID");
        }
    }
}
