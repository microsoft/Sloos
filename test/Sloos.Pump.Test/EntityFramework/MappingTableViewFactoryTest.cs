// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingTableViewFactoryTest
    {
        private readonly EntityTypeBuilder builder = new EntityTypeBuilder();

        [Fact]
        public void FactoryShouldHaveCorrectMetadata()
        {
            var entityType = this.builder
                .Name("My.Table")
                .WithProperty<int>("ID")
                .WithMetadataProperty<string>("Table", "Tables")
                .WithMetadataProperty<string>("Schema", "dbo")
                .Build();

            var testSubject = new MappingTableViewFactory();
            
            var mappingTableView = testSubject.Create(entityType);
            mappingTableView.Table.Should().Be("Tables");
            mappingTableView.Schema.Should().Be("dbo");
        }
    }
}
