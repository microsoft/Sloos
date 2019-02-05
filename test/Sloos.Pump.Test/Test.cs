// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Design;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Xml;
using Xunit;

namespace Sloos.Pump.Test
{
    public class Test
    {
        private static readonly IEnumerable<EntityStoreSchemaFilterEntry> StoreMetadataFilters = new[]
        {
            new EntityStoreSchemaFilterEntry(null, null, "EdmMetadata", EntityStoreSchemaFilterObjectTypes.Table, EntityStoreSchemaFilterEffect.Exclude),
            new EntityStoreSchemaFilterEntry(null, null, "__MigrationHistory", EntityStoreSchemaFilterObjectTypes.Table, EntityStoreSchemaFilterEffect.Exclude)
        };

        //[Fact]
        [Fact(Skip = "Debug only.")]
        [Trait("IsInteractive","true")]
        public void MyTest()
        {
            var names = new[] { "Simple", "Strings", "AllClrTypes", "GuidIdentity", "DateTime", "CompositeKey", "InverseMapping", "OneToMany", "ManyToMany", "AdventureWorks" };
            //var reverse = new ReverseEngineerCodeFirstHandler();

            foreach (var name in names)
            {
                // Load store schema
                var storeGenerator = new EntityStoreSchemaGenerator(
                    "System.Data.SqlClient",
                    $"Server=localhost;Database={name};Trusted_Connection=True;",
                    "dbo");

                storeGenerator.GenerateForeignKeyProperties = true;
                var errors = storeGenerator.GenerateStoreMetadata(Test.StoreMetadataFilters)
                    .Where(x => x.Severity == System.Data.Metadata.Edm.EdmSchemaErrorSeverity.Error)
                    .ToArray();

                storeGenerator.WriteStoreSchema(@"c:\temp\" + name + ".ssdl");

                //errors.HandleErrors(Strings.ReverseEngineer_SchemaError);
                var reader = XmlReader.Create(@"c:\temp\" + name + ".ssdl");

                IList<EdmSchemaError> theErrors;
                var storeItemCollection = StoreItemCollection.Create(new[] { reader }, null, null, out theErrors);

                //var storeItemCollection = new System.Data.Metadata.Edm.StoreItemCollection(new[] { reader });
                //var blahCC = new EntityModelSchemaGenerator(storeItemCollection, "My", "Default");
                //blahCC.PluralizationService = PluralizationService.CreateService(new CultureInfo("en"));
                //blahCC.GenerateForeignKeyProperties = true;
                //blahCC.GenerateMetadata();

                //MetadataWorkspace workspace = new MetadataWorkspace(
                //    () => storeItemCollection,
                //    null,
                //    null);

                var blahB1 = storeItemCollection.GetEntityContainer("dboContainer");
                var blahB2 = storeItemCollection.OfType<EntityType>().ToArray();

                // Generate default mapping
                var contextName = "MyContext";
                var modelGenerator = new EntityModelSchemaGenerator(storeGenerator.EntityContainer, "DefaultNamespace", contextName);
                modelGenerator.PluralizationService = PluralizationService.CreateService(new CultureInfo("en"));
                modelGenerator.GenerateForeignKeyProperties = true;
                modelGenerator.GenerateMetadata();

                modelGenerator.WriteModelSchema(@"C:\temp\" + name + ".csdl");
                modelGenerator.WriteStorageMapping(@"c:\temp\" + name + ".msdl");
            }

            // Pull out info about types to be generated
            //var entityTypes = modelGenerator.EdmItemCollection.OfType<EntityType>().ToArray();

            //var mappings = new ReverseEngineerCodeFirstHandler.EdmMapping(modelGenerator, storeItemCollection);

            //var contextHost = new EfTextTemplateHost
            //{
            //    EntityContainer = modelGenerator.EntityContainer,
            //    Namespace = "MyNamespace",
            //    ModelsNamespace = "MyNamespace.Model",
            //    MappingNamespace = "MyNamespace.Mapping",
            //    EntityFrameworkVersion = new Version(5,0),
            //};

            //var templateProcessor = new TemplateProcessor();
            //var contextContents = templateProcessor.ProcessContext(contextHost);

            //foreach (var entityType in entityTypes)
            //{
            //    // Generate the code file
            //    var entityHost = new EfTextTemplateHost
            //        {
            //            EntityType = entityType,
            //            EntityContainer = modelGenerator.EntityContainer,
            //            Namespace = "MyNamespace",
            //            ModelsNamespace = "MyNamespace.Model",
            //            MappingNamespace = "MyNamespace.Mapping",
            //            EntityFrameworkVersion = new Version(5, 0),
            //            TableSet = mappings.EntityMappings[entityType].Item1,
            //            PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
            //            ManyToManyMappings = mappings.ManyToManyMappings
            //        };

            //    var entityContents = templateProcessor.ProcessEntity(entityHost);
            //}

            //foreach (var entityType in entityTypes)
            //{
            //    var mappingHost = new EfTextTemplateHost
            //        {
            //            EntityType = entityType,
            //            EntityContainer = modelGenerator.EntityContainer,
            //            Namespace = "MyNamespace",
            //            ModelsNamespace = "MyNamespace.Model",
            //            MappingNamespace = "MyNamespace.Mapping",
            //            EntityFrameworkVersion = new Version(5, 0),
            //            TableSet = mappings.EntityMappings[entityType].Item1,
            //            PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
            //            ManyToManyMappings = mappings.ManyToManyMappings
            //        };

            //    var mappingContents = templateProcessor.ProcessMapping(mappingHost);
            //}
            //reverse.ReverseEngineerCodeFirst();
            Assert.True(true);
        }

        [Fact(Skip = "Debug only.")]
        [Trait("IsInteractive", "true")]
        public void WriteStoreSchema()
        {
            var catalogs = new[]
            {
                "AllClrTypes",
                "AdventureWorks",
                "ComplexType",
                "CompositeKey",
                "DateTime",
                "GuidIdentity",
                "InverseMapping",
                "ManyToMany",
                "OneToMany",
                "Strings",
            };

            foreach (var catalog in catalogs)
            {
                var connectionString = $"Server=localhost;Database={catalog};Trusted_Connection=True;";
                var fileName = $@"c:\dev\sloos\trunk\test\Sloos.Pump.Test\Samples\CodeFirstGen\{catalog}.xml";

                // Load store schema
                var storeGenerator = new EntityStoreSchemaGenerator(
                    "System.Data.SqlClient",
                    connectionString,
                    "dbo");

                storeGenerator.GenerateForeignKeyProperties = true;
                // BLAH:
                ////var errors = storeGenerator.GenerateStoreMetadata(_storeMetadataFilters)
                ////    .Where(x => x.Severity == System.Data.Metadata.Edm.EdmSchemaErrorSeverity.Error);

                ////Assert.Empty(errors);
                storeGenerator.WriteStoreSchema(fileName);
            }
        }
    }
}
