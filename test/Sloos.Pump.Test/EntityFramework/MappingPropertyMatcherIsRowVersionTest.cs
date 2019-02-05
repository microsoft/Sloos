// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyMatcherIsRowVersionTest
    {
        private readonly EdmPropertyBuilder builder = new EdmPropertyBuilder();

        [Fact]
        public void IsRowVersionShouldReturnTrue()
        {
            var edmProperty = this.builder.BuildBinary("prop", true, 8);
            edmProperty.StoreGeneratedPattern = StoreGeneratedPattern.Computed;
            
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsRowVersion();
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
        }

        [Fact]
        public void IsRowVersionShouldReturnFalse()
        {
            var edmProperty = this.builder.BuildBinary("prop", false, 8);
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsRowVersion();
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }
    }
}
