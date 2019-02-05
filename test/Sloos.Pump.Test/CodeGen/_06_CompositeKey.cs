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
    public class _06_CompositeKey
    {
        [Fact]
        public void Reverse()
        {
            var entityNamespace = new EntityNamespace("Fact");

            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.CompositeKey.Csdl(),
                CodeFirstGen.CompositeKey.Ssdl(),
                CodeFirstGen.CompositeKey.Msdl());

            var factory = new ReverseFactory(
                sampleGeneratorFactory,
                entityNamespace,
                "Context");

            ApprovalsExtensions.Verify(factory);
        }
    }
}
