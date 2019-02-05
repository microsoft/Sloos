// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Builder;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class MappingPropertyMatcherIsUnicodeTest
    {
        private readonly EdmPropertyBuilder builder = new EdmPropertyBuilder();

        [Fact]
        public void IsUnicodeTypeStringShouldReturnTrue()
        {
            var edmProperty = this.builder.BuildString("prop", true, false);
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsUnicode();
            testSubject.IsMatch(mappingProperty).Should().BeTrue();
        }

        [Fact]
        public void IsUnicodeTypeStringShouldReturnFalse()
        {
            var edmProperty = this.builder.BuildString("prop", false, false);
            var mappingProperty = new MappingProperty(edmProperty);

            var testSubject = new MappingPropertyMatcherIsUnicode();
            testSubject.IsMatch(mappingProperty).Should().BeFalse();
        }
    }
}
