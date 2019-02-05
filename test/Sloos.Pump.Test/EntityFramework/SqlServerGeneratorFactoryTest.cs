// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

// Copyright(c) Microsoft.All rights reserved.
// Licensed under the MIT license.

using System.IO;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Sloos.Common.Build;
using Sloos.Pump.EntityFramework;
using Xunit;

namespace Sloos.Pump.Test.EntityFramework
{
    public class SqlServerGeneratorFactoryTest
    {
        [Fact(Skip = "Broken.")]
        [Trait("Category", "Slow")]
        public void _08_ManyToMany()
        {
            const string connectionString = "Server=localhost;Database=ManyToMany;Trusted_Connection=true";
            var sqlServerGenerator = new SqlServerGeneratorFactory(
                connectionString);

            var reverseFactory = new ReverseFactory(
                sqlServerGenerator,
                new EntityNamespace("Fact"),
                "Context");

            var code = new StringBuilder();
            reverseFactory.WriteTo(code);

            var path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            var compiler = new CompileContext(
                code.ToString(),
                Path.Combine(path, @"..\..\..\..\packages\EntityFramework.6.2.0\lib\net45\EntityFramework.dll"),
                Path.Combine(path, @"..\..\..\..\packages\EntityFramework.6.2.0\lib\net45\EntityFramework.SqlServer.dll"));

            compiler.Assembly.Should().NotBeNull();
            Assembly.Load(compiler.Assembly.FullName)
                .Should().NotBeNull();

            var contextType = compiler.Assembly.GetType("Fact.Models.Context");
            contextType.Should().NotBeNull();
        }
    }
}
