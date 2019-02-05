// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyMatcherIsRequiredTest
    {
        private readonly EdmPropertyBuilder builder = new EdmPropertyBuilder();

        [Fact]
        public void IsRequiredTypeStringShouldReturnTrue()
        {
            var edmProperty = this.builder
                .BuildString("prop", false, false);

            edmProperty.Nullable = false;
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsRequired();
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
        }

        [Fact]
        public void IsRequiredTypeStringShouldReturnFalse()
        {
            var edmProperty = this.builder
                .BuildString("prop", false, false);

            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsRequired();
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }

        [Fact]
        public void IsRequiredTypeInt32ShouldReturnFalse()
        {
            var edmProperty = this.builder.Build<int>("prop");
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsRequired();
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }
    }
}
