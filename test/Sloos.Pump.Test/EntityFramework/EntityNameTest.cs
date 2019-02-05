// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class EntityNameTest
    {
        [Fact]
        public void PluralNameShouldBeCorrectlyConverted()
        {
            var testSubject = new EntityName("Tables");
            testSubject.ConceptualName.Should().Be("Table");
            testSubject.ConceptualMappingName.Should().Be("TableMap");
            testSubject.StoreName.Should().Be("Tables");
        }

        [Fact]
        public void SingularNameShouldBeCorrectlyConverted()
        {
            var testSubject = new EntityName("Table");
            testSubject.ConceptualName.Should().Be("Table");
            testSubject.ConceptualMappingName.Should().Be("TableMap");
            testSubject.StoreName.Should().Be("Tables");
        }
    }
}
