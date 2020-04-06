// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.EntityFrameworkCore;

namespace Sloos.Common.Build
{
    public sealed class CompileContext
    {
        public CompileContext(
            string code,
            params string[] assemblyPaths)
        {
            this.Assembly = this.BuildAssembly(code, assemblyPaths);
        }

        public Assembly Assembly { get; }

        public Type GetContextType()
        {
            return this.Assembly
                .GetTypes()
                .Where(x => x.IsPublic)
                .First(x => x.IsSubclassOf(typeof(DbContext)));
        }
        
        public DbContext Create(string nameOrConnectionString)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string name)
        {
            return this.Assembly.GetType(name);
        }

        private Assembly BuildAssembly(
            string code,
            params string[] assemblyPaths)
        {
            var name = $"CodeGen.{Guid.NewGuid():N}";

            using (var codeProvider = new CSharpCodeProvider(
               new Dictionary<string, string> { { "CompilerVersion", "v4.0" } }))
            {
                var assemblies = new List<string>
                {
                    "mscorlib.dll",
                    "System.dll",
                    "System.ComponentModel.DataAnnotations.dll",
                    "System.Core.dll",
                    "System.Data.Entity.dll",
                    "System.Runtime.Serialization.dll",
                };

                assemblies.AddRange(assemblyPaths);

                var options = new CompilerParameters(
                    assemblies.ToArray())
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = true,
                    OutputAssembly = $"{name}.dll",
                };

                CompilerResults results = codeProvider.CompileAssemblyFromSource(options, code);

                if (results.Errors.Count > 0)
                {
                    var message = $"Cannot compile typed context: {results.Errors[0].ErrorText} (line {results.Errors[0].Line})";
                    throw new Exception(message);
                }

                return results.CompiledAssembly;
            }
        }
    }
}
