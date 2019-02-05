// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Common;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyTest
    {
        private readonly EdmPropertyBuilder builder = new EdmPropertyBuilder();

        [Fact]
        public void DecimalShouldBeNumericType()
        {
            var edmProperty = this.builder.Build<decimal>("prop");
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfNumericKey().Should().BeTrue();
        }

        [Fact]
        public void Int16ShouldBeNumericType()
        {
            var edmProperty = this.builder.Build<short>("prop");
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfNumericKey().Should().BeTrue();
        }

        [Fact]
        public void Int32ShouldBeNumericType()
        {
            var edmProperty = this.builder.Build<int>("prop");
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfNumericKey().Should().BeTrue();
        }

        [Fact]
        public void Int64ShouldBeNumericType()
        {
            var edmProperty = this.builder.Build<long>("prop");
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfNumericKey().Should().BeTrue();
        }

        [Fact]
        public void IsFixedLength()
        {
            var edmProperty = this.builder.BuildString("prop", false, true);
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfString().Should().BeTrue();
            mappingProperty.HasFacet(DbProviderManifest.FixedLengthFacetName).Should().BeTrue();
            mappingProperty.GetFacetValue<bool>(DbProviderManifest.FixedLengthFacetName).Should().BeTrue();
        }

        [Fact]
        public void IsStoreGeneratedComputedPatternShouldBeTrue()
        {
            var edmProperty = this.builder.Build<int>("prop");
            edmProperty.StoreGeneratedPattern = System.Data.Entity.Core.Metadata.Edm.StoreGeneratedPattern.Computed;

            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsStoreGeneratedComputed.Should().BeTrue();
        }

        [Fact]
        public void IsStoreGeneratedIdenittyPatternShouldBeTrue()
        {
            var edmProperty = this.builder.Build<int>("prop");
            edmProperty.StoreGeneratedPattern = System.Data.Entity.Core.Metadata.Edm.StoreGeneratedPattern.Identity;

            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsStoreGeneratedIdentity.Should().BeTrue();
        }

        [Fact]
        public void MaxLength()
        {
            var edmProperty = this.builder.BuildString("prop", false, false, 100);
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfString().Should().BeTrue();
            mappingProperty.HasFacet(DbProviderManifest.FixedLengthFacetName).Should().BeTrue();
            mappingProperty.GetFacetValue<bool>(DbProviderManifest.FixedLengthFacetName).Should().BeFalse();

            mappingProperty.HasFacet(DbProviderManifest.MaxLengthFacetName).Should().BeTrue();
            mappingProperty.GetFacetValue<int>(DbProviderManifest.MaxLengthFacetName).Should().Be(100);
        }

        [Fact]
        public void MaxLengthBinary()
        {
            var edmProperty = this.builder.BuildBinary("prop", false, 100);
            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsTypeOfByteArray().Should().BeTrue();
            mappingProperty.HasFacet(DbProviderManifest.FixedLengthFacetName).Should().BeTrue();
            mappingProperty.GetFacetValue<bool>(DbProviderManifest.FixedLengthFacetName).Should().BeFalse();

            mappingProperty.HasFacet(DbProviderManifest.MaxLengthFacetName).Should().BeTrue();
            mappingProperty.GetFacetValue<int>(DbProviderManifest.MaxLengthFacetName).Should().Be(100);
        }

        [Fact]
        public void Required()
        {
            var edmProperty = this.builder.BuildString("prop", false, false);
            edmProperty.Nullable = false;

            var mappingProperty = new MappingProperty(edmProperty);

            mappingProperty.IsNullable.Should().BeFalse();
            mappingProperty.GetClrEquivalentType().Should().Be(typeof(string));
            mappingProperty.IsTypeOfString().Should().BeTrue();
            mappingProperty.IsNullable.Should().BeFalse();
        }
    }
}
