// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Bricelam.EntityFrameworkCore.Design;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace Sloos
{
    public class Context : DbContext
    {
        public Context(string nameOfConnectionString)
            : base((new DbContextOptionsBuilder())
                  .UseSqlServer(nameOfConnectionString)
                  .Options)
        {
        }

        public Context(DbContextOptions options)
            : base(options)
        {
        }
    }

    public class EntityPump
    {
        public string TableName { get; set; }
        public Type EntityType { get; set; }
        public Type ContextType { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public string TypeName { get; set; }
    }

    class ContextFactory
    {
        private string code;
        private string tableName;
        private string rowName;

        public ContextFactory(
            string typeName,
            Column[] columns)
        {
            this.Construct(typeName, columns);
        }

        public ContextFactory(
            string typeName,
            string schema)
        {
            var columns = schema
                .Split(',')
                .Select(x => x.Split(':'))
                .Select((x, i) => new Column
                {
                    Name = x[0],
                    TypeName = x[1],
                    Offset = i,
                })
                .ToArray();

            this.Construct(typeName, columns);
        }

        private bool IsPlural(Pluralizer pluralizer, string word)
        {
            var method = pluralizer.GetType().GetMethod(
                "IsPlural", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)method.Invoke(pluralizer, new object[] { word });
        }

        private void Construct(string typeName, Column[] columns)
        {
            // NOTE(chrboum) :: one does not blindly include Bricelam.EntityFrameworkCore. 
            // To enable this code you must edit the project's .csproj, locate the package
            // reference, and then delete the following lines.
            //
            //  <PrivateAssets>all</PrivateAssets>
            //  < IncludeAssets > runtime; build; native; contentfiles; analyzers; buildtransitive </ IncludeAssets >
            //
            var pluralizer = new Pluralizer();
            if (this.IsPlural(pluralizer, typeName))
            {
                this.rowName = pluralizer.Singularize(typeName);
                this.tableName = typeName;
            }
            else
            {
                this.rowName = typeName;
                this.tableName = pluralizer.Pluralize(typeName);
            }

            this.code = ContextTextTemplate.Template(
                "Spike.CodeGen",
                this.rowName,
                this.tableName,
                "ID",
                columns);
        }

        public EntityPump BuildAssembly(
            AssemblyName assemblyName)

        {
            var options = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                reportSuppressedDiagnostics: true,
                optimizationLevel: OptimizationLevel.Release,
                generalDiagnosticOption: ReportDiagnostic.Default);

            var executingAssembly = Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(executingAssembly.Location);

            var references = new string[]
            {
                // .NET Core core assemblies
                typeof(object).Assembly.Location,                                   // System.Object
                Assembly.Load("netstandard, Version=2.0.0.0").Location,             
                Assembly.Load("System.Runtime").Location,                           
                typeof(Compilation).Assembly.Location,                              // CodeAnalysis
                typeof(Queryable).Assembly.Location,                                // System.Linq
                typeof(IQueryable).Assembly.Location,                               // System.Linq.Expressions
                typeof(IAsyncEnumerable<>).Assembly.Location,                       // System.Runtime ??
                Assembly.Load("Microsoft.Bcl.AsyncInterfaces").Location,            
                typeof(IServiceProvider).Assembly.Location,                         // System.ComponentModel
                typeof(KeyAttribute).Assembly.Location,                             // System.ComponentModel.DataAnnotation
                typeof(System.ComponentModel.IListSource).Assembly.Location,        // System.ComponentModel.TypeConverter
                typeof(DataContractAttribute).Assembly.Location,                    // System.Runtime.Serialization
                typeof(DbException).Assembly.Location,                              // System.Data.Common

                executingAssembly.Location,                                         // Sloos (IEntityPump)
                Path.Combine(path, "Microsoft.EntityFrameworkCore.dll"),            // EntityFramework
                Path.Combine(path, "Microsoft.EntityFrameworkCore.Design.dll"),     // EntityFramework
                Path.Combine(path, "Microsoft.EntityFrameworkCore.Relational.dll"), // EntityFramework
                Path.Combine(path, "System.Data.SqlClient.dll"),                    // SqlClient
            }
            .Select(x => MetadataReference.CreateFromFile(x))
            .ToArray();

            var compilation = CSharpCompilation.Create(
                assemblyName.Name,
                syntaxTrees: new SyntaxTree[] {  CSharpSyntaxTree.ParseText(this.code) },
                references: references,
                options: options);

            using (var stream = new MemoryStream())
            {
                var results = compilation.Emit(stream);
                if (!results.Success)
                {
                    var sb = new StringBuilder();
                    var errors = results.Diagnostics.Length == 1 ? "error" : "errors";
                    var errorCount = Math.Max(results.Diagnostics.Length, 10);
                    sb.AppendLine($@"Cannot compile typed context({results.Diagnostics.Length} {errors})");
                    sb.AppendLine($@">>> TOP {errorCount} {errors.ToUpper()} <<<");
                    for(int i=0; i < errorCount; i++)
                    {
                        sb.AppendLine($@"[{i}] :: {results.Diagnostics[i]}");
                    }

                    throw new Exception(sb.ToString());
                }

                var assembly = Assembly.Load(stream.ToArray());
                var entityType = assembly.DefinedTypes.First(x => x.Name == this.rowName);
                var contextType = assembly.DefinedTypes.First(x => x.Name == "Context");

                var pump = new EntityPump()
                {
                    TableName = this.tableName,
                    EntityType = entityType,
                    ContextType = contextType,
                };

                return pump;
            }
        }
    }
}
