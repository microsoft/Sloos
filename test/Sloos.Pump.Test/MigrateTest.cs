// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Sloos.Common.Build;
using Sloos.Pump.EntityFramework;
using Sloos.Pump.Test.EntityFramework;
using Sloos.Pump.Test.Samples;
using Xunit;

namespace Sloos.Pump.Test
{
    public class MigrateTest
    {
        //[Fact(Skip="Moved to Core, but test hasn't updated.")]
        public void DatabaseShouldDeployFromReverseEngineer()
        {
            var entityNamespace = new EntityNamespace("Fact");

            var sampleGeneratorFactory = new SampleGeneratorFactory(
                CodeFirstGen.Simple.Csdl(),
                CodeFirstGen.Simple.Ssdl(),
                CodeFirstGen.Simple.Msdl());

            var factory = new ReverseFactory(
                sampleGeneratorFactory,
                entityNamespace,
                "Context");

            var code = new StringBuilder();
            factory.WriteTo(code);

            var path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            var compiler = new CompileContext(
                code.ToString(),
                Path.Combine(path, @"..\..\..\..\packages\EntityFramework.6.2.0\lib\net45\EntityFramework.dll"),
                Path.Combine(path, @"..\..\..\..\packages\EntityFramework.SqlServerCompact.6.2.0\lib\net45\EntityFramework.SqlServerCompact.dll"));

            var contextType = compiler.GetContextType();

            var sqlCompactContextFactory = new SqlCompactContextFactory();

            using (var context = sqlCompactContextFactory.Create(x => (DbContext)Activator.CreateInstance(contextType, x)))
            {
                context.Database.CreateIfNotExists();

                var tableType = compiler.GetType("Fact.Models.Table");
                var table1 = (dynamic)Activator.CreateInstance(tableType);
                table1.Name = "Donkey";

                dynamic dc = context;
                dc.Tables.Add(table1);
                dc.SaveChanges();
            }

            using (dynamic context = sqlCompactContextFactory.Create(x => (DbContext)Activator.CreateInstance(contextType, x)))
            {
                var name = (string)System.Linq.Queryable.First(context.Tables).Name;
                name.Should().Be("Donkey");
            }
        }
    }
}
