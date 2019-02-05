// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Pump.Test.Builder
{
    public class EntityTypeBuilderTest
    {
        [Fact]
        public void Test()
        {
            var builder = new EntityTypeBuilder();
            var entity = builder
                .Name("My.Table")
                .WithProperty<int>("ID")
                .WithKeys("ID")
                .Build();

            entity.Should().NotBeNull();
        }
    }

    public class EntityTypeBuilder : IEntityFrameworkBuilder<EntityType>
    {
        private readonly EdmPropertyBuilder builder;

        private readonly DataSpace dataSpace;

        private readonly List<EdmProperty> edmProperties;

        private readonly List<string> keyMemberNames;

        private readonly List<MetadataProperty> metadataProperties;

        private readonly TypeUsageFactory typeUsageFactory;

        private string entityName;
        private string entityNamespace;

        public EntityTypeBuilder()
        {
            this.dataSpace = DataSpace.CSpace;
            this.builder = new EdmPropertyBuilder();
            this.typeUsageFactory = new TypeUsageFactory();

            this.keyMemberNames = new List<string>();
            this.edmProperties = new List<EdmProperty>();
            this.metadataProperties = new List<MetadataProperty>();
        }

        public EntityType Build()
        {
            var entityType = EntityType.Create(
                this.entityName,
                this.entityNamespace,
                this.dataSpace,
                this.keyMemberNames,
                this.edmProperties,
                this.metadataProperties);

            this.keyMemberNames.Clear();
            this.edmProperties.Clear();
            this.metadataProperties.Clear();

            return entityType;
        }

        public EntityTypeBuilder Name(string name)
        {
            var split = name.Split('.');
            this.entityName = split.Last();
            this.entityNamespace = string.Join(".", split.Reverse().Skip(1).Reverse());

            return this;
        }

        public EntityTypeBuilder WithProperty<T>(string name)
        {
            this.edmProperties.Add(
                this.builder.Build<T>(name));
            
            return this;
        }

        public EntityTypeBuilder WithProperty<T>(string name, Action<EdmProperty> action) 
        {
            this.edmProperties.Add(
                this.builder.Build<T>(name, action));

            return this;
        }

        public EntityTypeBuilder WithKeys(params string[] keys)
        {
            this.keyMemberNames.AddRange(keys);
            return this;
        }

        public EntityTypeBuilder WithMetadataProperty<T>(string name, object value)
        {
            this.metadataProperties.Add(
                MetadataProperty.Create(name, this.typeUsageFactory.Create<T>(), value));

            return this;
        }
    }
}
