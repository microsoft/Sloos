// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using System.Xml.Serialization;
using FluentAssertions;
using Sloos.Pump.SsdlSchema.Xsd;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test
{
    public class DeserializeSimpleTest
    {
        [Fact]
        public void Deserialize()
        {
            var serializer = new XmlSerializer(typeof(TSchema));
            var schema = (TSchema)serializer.Deserialize(CodeFirstGen.Simple.Ssdl());

            schema.Should().NotBeNull();
            schema.EntityContainer.Should().HaveCount(1);
            
            var entityContainer = schema.EntityContainer.First();
            entityContainer.Items.Should().HaveCount(1);

            var entitySet = entityContainer.Items[0] as EntityContainerEntitySet;
            entitySet.Should().NotBeNull();
            entitySet.Name.Should().Be("Tables");
            entitySet.EntityType.Should().Be("dbo.Tables");

            schema.EntityType.Should().HaveCount(1);
            var entityType = schema.EntityType[0];

            entityType.Key.PropertyRef.Should().HaveCount(1);
            entityType.Key.PropertyRef[0].Name.Should().Be("ID");

            entityType.Items.Should().HaveCount(2);
            entityType.Items[0].Name.Should().Be("ID");
            entityType.Items[0].Type.Should().Be("int");
            entityType.Items[0].Nullable.Should().BeFalse();
            entityType.Items[0].StoreGeneratedPatternSpecified.Should().BeTrue();
            entityType.Items[0].StoreGeneratedPattern.Should().Be(TStoreGeneratedPattern.Identity);

            entityType.Items[1].Name.Should().Be("Name");
            entityType.Items[1].Type.Should().Be("nvarchar(max)");
        }
    }
}
