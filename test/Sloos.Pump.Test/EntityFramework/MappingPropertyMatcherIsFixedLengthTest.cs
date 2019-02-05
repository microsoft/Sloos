// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyMatcherIsFixedLengthTest
    {
        private readonly EdmPropertyBuilder builder = new EdmPropertyBuilder();

        [Fact]
        public void IsFixedLengthStringShouldBeTrue()
        {
            var edmProperty = this.builder
                .BuildString("prop", false, true, 128);
            
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsFixedLength();
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
        }

        [Fact]
        public void IsFixedLengthStringShouldBeFalse()
        {
            var edmProperty = this.builder
                .BuildString("prop", false, false);

            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsFixedLength();
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }
    }
}
