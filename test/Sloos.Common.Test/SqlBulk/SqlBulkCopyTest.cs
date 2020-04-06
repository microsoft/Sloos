// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Xunit;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// This class is the culmination of the work for the CSV serializer.  It
// demonstrates how to do move data a CSV document to a SQL DB in bulk
// transfer mode.
//
// Step 1.
// Define an entity class to represent a row of your data.
//
// Step 2.
// Define a context via EntityFramework to facilitate communication with
// a DB.  This makes it easy to query the database, and create it 
// automatically if it does not exit.
//
// Step 3.
// Parse the CSV document, and then bulk copy it in a SQL DB.  The code
// uses the work from David Browne to move the entities into the DB.
// (DelimitedParser is also an IDataReader, but I think working at the
// entity level is better.)
//
// Step 4.
// Use the EntityFramework to read the data back of the DB, and party
// on it.
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

namespace Sloos.Common.Test
{
    // Create an entity to represent a "row" of the Cosmos output.  It serves
    // double duty as the database entity.
    [DataContract]
    public class WebMetric
    {
        // This field is ignored by the CsvSerializer, but is used by EntityFramework
        // to ensure the table has a primary key.
        public long ID { get; set; }

        // Property for each column in the "row" of the Cosmos output.  
        // Use the [Required] attribute to avoid the creation of nullable types,
        // i.e. you want double not double? for Min.
        //
        // Uri, Count, Min, P25, P50, P75, P99, Max
        // http://support.microsoft.com/kb/100, 100, 0.01, 0.25, 0.5, 0.75, 2.1, 5.01
        [DataMember(Order = 0)] [Required] public string Uri { get; set; }
        [DataMember(Order = 1)] [Required] public long Count { get; set; }
        [DataMember(Order = 2)] [Required] public double Min { get; set; }
        [DataMember(Order = 3)] [Required] public double P25 { get; set; }
        [DataMember(Order = 4)] [Required] public double P50 { get; set; }
        [DataMember(Order = 5)] [Required] public double P75 { get; set; }
        [DataMember(Order = 6)] [Required] public double P99 { get; set; }
        [DataMember(Order = 7)] [Required] public double Max { get; set; }
    }

    // TODO(chrboum) :: port .NET Core 3.1 and EF.Core
    // Create a context to create our database, and declare the tables that exist.
    //public class Context : DbContext
    //{
    //    static Context()
    //    {
    //        Database.SetInitializer(new ContextInitializer());
    //    }

    //    public Context(string nameOrConnectionString)
    //        : base (nameOrConnectionString)
    //    {
    //    }

    //    public IDbSet<WebMetric> WebMetrics { get; set; } 

    //    // Create the DB if it does not exist.  It will never exist because I use
    //    // a random file name every time this test is run.
    //    internal class ContextInitializer : CreateDatabaseIfNotExists<Context>
    //    {
    //    }
    //}

    // !!! EXAMPLES !!!
    // Everyone loves examples!

    //public class SqlBulkCopyTest
    //{
    //    // Bulk loading of data using CsvSerializer.
    //    [Fact]
    //    [Trait("Category", "Slow")]
    //    public void SqlBulkCopy_CsvSerializer()
    //    {
    //        string path = Path.Combine(
    //            Directory.GetCurrentDirectory(),
    //            Path.GetRandomFileName());
    //        path = Path.ChangeExtension(path, "mdf");

    //        // Use LocalDB as the temporary database for the test.  Substitute with a
    //        // connection string more appropriate to your environment.
    //        string connectionString = $@"Server=(localdb)\MSSQLLocalDB;AttachDbFilename={path};Trusted_Connection=True";

    //        // Build up some fake Cosmos output.
    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendLine("http://support.microsoft.com/kb/100,100,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("http://support.microsoft.com/kb/101,500,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("http://support.microsoft.com/kb/200,700,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("http://support.microsoft.com/kb/321,902,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("http://support.microsoft.com/kb/732,199,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("http://support.microsoft.com/kb/376,112,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("http://support.microsoft.com/kb/546,414,0.01,0.25,0.5,0.75,2.1,5.01");

    //        // Read in the fake Cosmos output.  Instead of using this stream, why not use
    //        //  -> new CosmosStream("http://cosmos05.osdinfra.net:88/cosmos/0365exp.adhoc/my/stream.csv");
    //        var serializer = new CsvSerializer<WebMetric>();
    //        var metrics = serializer.Deserialize(sb.ToStream());

    //        // Ensure the database is created before bulk loading the data.
    //        using (var context = new Context(connectionString))
    //        {
    //            Assert.False(context.WebMetrics.Any());
    //        }

    //        // NOTE: Using snapshot is advised, but LocalDB does not support it.
    //        //const IsolationLevel isolationLevel = IsolationLevel.Snapshot;
    //        const IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;

    //        // Bulk load the data.
    //        using (var transactionScope = new TransactionScope(
    //            TransactionScopeOption.Required,
    //            new TransactionOptions { IsolationLevel = isolationLevel }))
    //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString))
    //        {
    //            sqlBulkCopy.DestinationTableName = "WebMetrics";
    //            sqlBulkCopy.BatchSize = 10000;

    //            sqlBulkCopy.WriteToServer(metrics.AsDataReader());
    //            transactionScope.Complete();
    //        }

    //        // Read data back out of the DB...party on it.
    //        using (var context = new Context(connectionString))
    //        {
    //            Assert.Equal(7, context.WebMetrics.Count());
                
    //            // There are seven unique IDs.
    //            var set = new HashSet<long>(context.WebMetrics.Select(x => x.ID));
    //            Assert.Equal(7, set.Count);

    //            Assert.True(context.WebMetrics.All(x => x.Min == 0.01));
    //            Assert.True(context.WebMetrics.All(x => x.Max == 5.01));
    //        }
    //    }

    //    // This is almost identical to CsvSerializer example except it utilizes 
    //    // DelimitedParser as an IDataReader.  There are differences to be aware of.
    //    //
    //    //  1. A column must be added to the data to account for the ID (primary key)
    //    //      -or-
    //    //     You must remove the ID from the entity, and not load it.
    //    //  2. You must defined the define the schema of columns ahead of time, of the
    //    //     form <name>:<type>.  The type is optional and defaults to string.
    //    //
    //    // There are potentially some benefits because you avoid having to parse
    //    // the CSV, and then convert the CSV to an entity, and then converting
    //    // the entities to a DataReader.  In the grand scheme of things the 
    //    // performance must likely exceeds that of the speed with which data can be
    //    // bulk loaded.
    //    [Fact]
    //    [Trait("Category", "Slow")]
    //    public void SqlBulkCopy_DelimitedParser()
    //    {
    //        string path = Path.Combine(
    //            Directory.GetCurrentDirectory(),
    //            Path.GetRandomFileName());
    //        path = Path.ChangeExtension(path, "mdf");

    //        // Use LocalDB as the temporary database for the test.  Substitute with a
    //        // connection string more appropriate to your environment.
    //        string connectionString = $@"Server=(localdb)\MSSQLLocalDB;AttachDbFilename={path};Trusted_Connection=True";

    //        // Build up some fake Cosmos output.
    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendLine("1,http://support.microsoft.com/kb/100,100,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("1,http://support.microsoft.com/kb/101,500,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("1,http://support.microsoft.com/kb/200,700,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("1,http://support.microsoft.com/kb/321,902,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("1,http://support.microsoft.com/kb/732,199,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("1,http://support.microsoft.com/kb/376,112,0.01,0.25,0.5,0.75,2.1,5.01");
    //        sb.AppendLine("1,http://support.microsoft.com/kb/546,414,0.01,0.25,0.5,0.75,2.1,5.01");

    //        // Read in the fake Cosmos output.  Instead of using this stream, why not use
    //        //  -> new CosmosStream("http://cosmos05.osdinfra.net:88/cosmos/0365exp.adhoc/my/stream.csv");

    //        var factory = new DelimitedColumnFactory();
    //        var delimitedHeader = new DelimitedHeader(
    //            factory.Create(new[] { "ID:long", "Uri", "Count:long", "Min:double", "P25:double", "P50:double", "P75:double", "P99:double", "Max:double" }));

    //        var settings = new DelimitedParserSettings();
    //        settings.DelimitedHeader = delimitedHeader;
    //        settings.Delimiter = ',';

    //        var parser = DelimitedParser.Create(settings, sb.ToStream());

    //        // Ensure the database is created before bulk loading the data.
    //        using (var context = new Context(connectionString))
    //        {
    //            Assert.False(context.WebMetrics.Any());
    //        }

    //        // Bulk load the data.
    //        using (var transactionScope = new TransactionScope(
    //            TransactionScopeOption.Required,
    //            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
    //        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString))
    //        {
    //            sqlBulkCopy.DestinationTableName = "WebMetrics";
    //            sqlBulkCopy.BatchSize = 10000;

    //            sqlBulkCopy.WriteToServer(parser);
    //            transactionScope.Complete();
    //        }

    //        // Read data back out of the DB...party on it.
    //        using (var context = new Context(connectionString))
    //        {
    //            Assert.Equal(7, context.WebMetrics.Count());

    //            // There are seven unique IDs.
    //            var set = new HashSet<long>(context.WebMetrics.Select(x => x.ID));
    //            Assert.Equal(7, set.Count);

    //            Assert.True(context.WebMetrics.All(x => x.Min == 0.01));
    //            Assert.True(context.WebMetrics.All(x => x.Max == 5.01));
    //        }
    //    }
    //}
}
