// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using FluentAssertions;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    /// <summary>
    /// Test to help me figure out how EdmMapping works.
    /// </summary>
    public class EdmMappingTest
    {
        [Fact]
        public void OneToManyShouldHaveOneRelationship()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.OneToMany.Csdl(),
                CodeFirstGen.OneToMany.Ssdl(),
                CodeFirstGen.OneToMany.Msdl());

            var edmMapping = sampleGeneratorFactory.EdmMapping;

            edmMapping.EntityMappings.Should().HaveCount(2);
            var entityMappingsKeys = edmMapping.EntityMappings.Keys.OrderBy(x => x.Name).ToArray();
            var entityMappingsKeyPost = entityMappingsKeys[0];
            var entityMappingsKeyTag = entityMappingsKeys[1];

            entityMappingsKeyPost.Name.Should().Be("Post");
            entityMappingsKeyTag.Name.Should().Be("Tag");

            // Posts
            var entityMappingsValuesPost = edmMapping.EntityMappings[entityMappingsKeyPost];
            entityMappingsValuesPost.Item1.Name.Should().Be("Posts");
            
            entityMappingsValuesPost.Item2.Should().HaveCount(1);
            var edmPropertyDict1 = entityMappingsValuesPost.Item2.Single();
            edmPropertyDict1.Key.Name.Should().Be("ID");
            edmPropertyDict1.Value.Name.Should().Be("ID");

            // Tags
            var entityMappingsValuesTag = edmMapping.EntityMappings[entityMappingsKeyTag];
            entityMappingsValuesTag.Item1.Name.Should().Be("Tags");

            entityMappingsValuesTag.Item2.Should().HaveCount(2);
            entityMappingsValuesTag.Item2.Keys.Select(x => x.Name).Should().Contain("ID", "Post_ID");
            entityMappingsValuesTag.Item2.Values.Select(x => x.Name).Should().Contain("ID", "Post_ID");
        }
    }
}
