// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class CodeGenEscaperTest
    {
        private readonly CodeGenEscaper escaper = new CodeGenEscaper();

        /// <summary>
        /// For those playing along at home, this is completely useless.  (Both
        /// my code, and the CSharpCodeProvider's code.
        /// </summary>
        [Fact]
        public void StringShouldBeEscaped()
        {
            this.escaper.Escape("dog").Should().Be("dog");
            this.escaper.Escape("the-dog").Should().Be("the-dog");
            this.escaper.Escape("the dog").Should().Be("the dog");
            this.escaper.Escape("string").Should().Be("@string");
        }

        [Fact]
        public void ComplexTypeShouldBeEscaped()
        {
            var edmType = ComplexType.Create("string", "Transient", DataSpace.CSSpace, Enumerable.Empty<EdmMember>(), null);
            var typeUsage = TypeUsage.Create(edmType, Enumerable.Empty<Facet>());

            this.escaper.Escape(typeUsage).Should().Be("@string");
        }

        [Fact]
        public void EntityTypeShouldBeEscaped()
        {
            var edmType = EntityType.Create("string", "Transient", DataSpace.CSSpace, null, null, null);
            var typeUsage = TypeUsage.Create(edmType, Enumerable.Empty<Facet>());

            this.escaper.Escape(typeUsage).Should().Be("@string");
        }

        [Fact]
        public void SimpleTypeShouldBeEscaped()
        {
            var edmType = PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String);
            var typeUsage = TypeUsage.CreateStringTypeUsage(edmType, false, false);

            this.escaper.Escape(typeUsage).Should().Be("global::System.String");
        }

        [Fact]
        public void SimpleNullableTypeShouldBeEscaped()
        {
            var primitiveType = PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32);

            // XXX:
            // I'm stupid and/or wrong.  Why do I have to jump through these hoops
            // to validate this code!?!
            var property = EdmProperty.CreatePrimitive("prop", primitiveType);
            property.Nullable = true;

            var nullableFacet = property
                .MetadataProperties
                .SelectMany(x => x.TypeUsage.Facets)
                .First(x => x.Name == System.Data.Entity.Core.Common.DbProviderManifest.NullableFacetName);

            var typeUsage = TypeUsage.Create(primitiveType, new[] { nullableFacet });
            this.escaper.Escape(typeUsage).Should().Be("global::System.Nullable<global::System.Int32>");
            // XXX
        }

        [Fact]
        public void EdmMemberShouldBeEscaped()
        {
            // TODO: consider creating a collection of default types suitable for testing,
            // so this massive setup is not always required.

            var entityTypeBuilder = new EntityTypeBuilder();

            var tags = entityTypeBuilder
                .Name("My.Tags")
                .WithProperty<int>("ID")
                .WithProperty<int>("Post_ID")
                .WithKeys("ID")
                .Build();

            var posts = entityTypeBuilder
                .Name("My.Posts")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();

            var builder = new NavigationPropertyBuilder();
            var navigationProperty = builder
               .FromRole(posts, RelationshipMultiplicity.Many)
               .ToRole(tags, RelationshipMultiplicity.One)
               .Map("ID", "Post_ID")
               .Build();

            this.escaper.Escape(navigationProperty).Should().Be("Tags");
        }

        [Fact]
        public void CollectionTypeShouldBeEscaped()
        {
            var primitive = PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String);
            var edmType = primitive.GetCollectionType();
            var typeUsage = TypeUsage.Create(edmType, Enumerable.Empty<Facet>());

            this.escaper.Escape(typeUsage).Should().Be("global::System.Collections.Generic.ICollection<global::System.String>");
        }

        [Fact]
        public void EscapeCollectionShouldFullyQualifyToICollectionOnly()
        {
            this.escaper.EscapeCollection("Foo").Should().Be("global::System.Collections.Generic.ICollection<Foo>");
        }

        [Fact]
        public void EscapeListShouldFullyQualifyToListOnly()
        {
            this.escaper.EscapeList("Foo").Should().Be("global::System.Collections.Generic.List<Foo>");
        }
    }
}
