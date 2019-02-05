// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using ApprovalTests.Reporters;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.EntityFramework;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class _08_ManyToMany
    {
        [Fact]
        public void Reverse()
        {
            var entityNamespace = new EntityNamespace("Fact");

            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.ManyToMany.Csdl(),
                CodeFirstGen.ManyToMany.Ssdl(),
                CodeFirstGen.ManyToMany.Msdl());

            var factory = new ReverseFactory(
                sampleGeneratorFactory,
                entityNamespace,
                "Context");

            ApprovalsExtensions.Verify(factory);
        }
    }
}
