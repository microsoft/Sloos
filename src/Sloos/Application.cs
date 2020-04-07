// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using FastMember;
using Microsoft.EntityFrameworkCore;

using Sloos.Common.Parse;
using Spike;

namespace Sloos
{
    public class Application
    {
        private readonly int batchSize;

        public Application(int batchSize)
        {
            this.batchSize = batchSize;
        }

        public int Pump(string table, string source, string connectionString)
        {
            var csvSerializerState = this.CreateSerializer(source);

            var columns = csvSerializerState.Names
                .Zip(csvSerializerState.Types, Tuple.Create)
                .Select((x, i) => new Column { Name = x.Item1, TypeName = x.Item2.FullName, Offset = i })
                .ToArray();

            var contextFactory = new ContextFactory(table, columns);
            var assemblyName = new AssemblyName($"Sloos.{Guid.NewGuid():N}.CodeGen")
            {
                CodeBase = Directory.GetCurrentDirectory(),
            };

            var pump = contextFactory.BuildAssembly(assemblyName);
            var dbContextOptions = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;


            using (var context = (IEntityPump)Activator.CreateInstance(pump.ContextType, dbContextOptions))
            {
                context.CreateIfNotExists();
            }

            Type listType = typeof(List<>).MakeGenericType(csvSerializerState.EntityType);
            var entities = (dynamic)Activator.CreateInstance(listType);

            Console.WriteLine($"[{DateTime.Now:o}] Parsing CSV ...");
            long rowCount = 0;
            Stopwatch completeTimer = Stopwatch.StartNew();

            dynamic serializer = csvSerializerState.Serializer;
            string[] columnNames = (new string[] { "ID" })
                .Concat((string[])serializer.ColumnNames)
                .ToArray();

            foreach (var row in serializer.Deserialize(File.OpenRead(source), csvSerializerState.Delimiter, 1))
            {
                // Commit in "bite-size" chunks...
                if (rowCount > 0 && rowCount % this.batchSize == 0)
                {
                    BulkLoadEntities(pump, connectionString, entities, columnNames);
                    entities.Clear();
                }

                entities.Add(row);
                rowCount++;
            }

            if (entities.Count > 0)
            {
                BulkLoadEntities(pump, connectionString, entities, columnNames);
            }

            Console.WriteLine($"[{DateTime.Now:o}]  -> completed -- {rowCount:N0} rows in {completeTimer.Elapsed}.");
            return 0;
        }

        private void BulkLoadEntities(EntityPump pump, string connectionString, dynamic entities, string[] columnNames)
        {
            Console.WriteLine($"[{DateTime.Now:o}] Preparing rows for commit ...");
            Stopwatch sw = Stopwatch.StartNew();

            using (var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {

                using (var sqlBulkCopy = new SqlBulkCopy(connectionString))
                {
                    sqlBulkCopy.DestinationTableName = pump.TableName;
                    sqlBulkCopy.BatchSize = 10000;
                    sqlBulkCopy.WriteToServer(ObjectReader.Create(entities, columnNames));

                    transactionScope.Complete();
                }
            }

            sw.Stop();
            Console.WriteLine($"[{DateTime.Now:o}]  -> committed {entities.Count:N0} rows in {sw.Elapsed}.");
        }

        private CsvSerializerState CreateSerializer(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                var factory = new CsvSerializerFactory();
                return factory.Create("MyAssembly", "MyEntity", stream);
            }
        }
    }
}
