// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyMatcherMaxLengthTest
    {
        private readonly EdmPropertyBuilder builder = new EdmPropertyBuilder();

        [Fact]
        public void MaxLengthStringShouldHaveMaxLength()
        {
            var edmProperty = this.builder.BuildString("prop", false, false, 128);
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherMaxLength();
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
            testSubject.GetMaxLength(mappingProperty).Should().Be(128);
        }

        [Fact]
        public void UnboundedStringShouldNotHaveMaxLength()
        {
            var edmProperty = this.builder.BuildString("prop", false, false);
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherMaxLength();
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }
    }
}
