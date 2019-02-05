// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using FluentAssertions;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class EntityEntityViewFactoryTest
    {
        [Fact]
        public void Test()
        {
            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.Simple.Csdl(),
                CodeFirstGen.Simple.Ssdl(),
                CodeFirstGen.Simple.Msdl());

            var entityType = sampleGeneratorFactory.StoreItemCollection.OfType<EntityType>()
                .Single(x => x.Name == "Tables");

            var testSubject = new EntityEntityViewFactory(new CodeGenEscaper());
            var views = testSubject.Create(entityType).ToArray();
            views.Should().HaveCount(2);

            views[0].Name.Should().Be("ID");
            views[0].TypeName.Should().Be("global::System.Int32");
            views[0].MethodModifier.Should().BeNullOrEmpty();

            views[1].Name.Should().Be("Name");
            views[1].TypeName.Should().Be("global::System.String");
            views[1].MethodModifier.Should().BeNullOrEmpty();
        }
    }
}
