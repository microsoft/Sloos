// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Sloos.Pump.EntityFramework
{
    public sealed class SqlServerGeneratorFactory : IGeneratorFactory
    {
        private static readonly string[] DefaultTablesToIgnore = { "EdmMetadata", "__MigrationHistory" };

        public SqlServerGeneratorFactory(
            string nameOrConnectionString,
            params string[] tableNames)
        {
            var tablesToIgnore = SqlServerGeneratorFactory.DefaultTablesToIgnore
                .Concat(tableNames)
                .Select(this.IgnoreTable);

            var entityStoreSchemaGenerator = SqlServerGeneratorFactory.CreateEntityStoreSchemaGenerator(
                nameOrConnectionString,
                tablesToIgnore);

            this.StoreItemCollection = SqlServerGeneratorFactory.LoadStoreItemCollection(
                entityStoreSchemaGenerator);

            var entityModelSchemaGenerator = SqlServerGeneratorFactory.CreateEntityModelSchemaGenerator(
                entityStoreSchemaGenerator.EntityContainer);

            this.ConceptualItemCollection = SqlServerGeneratorFactory.LoadEdmItemCollection(
                entityModelSchemaGenerator);

            this.EdmMapping = SqlServerGeneratorFactory.LoadEdmMapping(entityModelSchemaGenerator, this.ConceptualItemCollection, this.StoreItemCollection);
        }

        public StoreItemCollection StoreItemCollection { get; }

        public EdmItemCollection ConceptualItemCollection { get; }

        public EdmMapping EdmMapping { get; }

        private static EntityStoreSchemaGenerator CreateEntityStoreSchemaGenerator(
            string nameOrConnectionString,
            IEnumerable<EntityStoreSchemaFilterEntry> storeSchemaFilterEntries)
        {
            var entityStoreSchemaGenerator = new EntityStoreSchemaGenerator(
                "System.Data.SqlClient",
                nameOrConnectionString,
                "dbo");

            entityStoreSchemaGenerator.GenerateForeignKeyProperties = true;
            var errors = entityStoreSchemaGenerator.GenerateStoreMetadata(storeSchemaFilterEntries)
                .Where(x => x.Severity == System.Data.Metadata.Edm.EdmSchemaErrorSeverity.Error)
                .ToArray();

            if (errors.Any())
            {
                var exceptions = errors.Select(SqlServerGeneratorFactory.EdmSchemaErrorToException);
                throw new AggregateException(exceptions);
            }

            return entityStoreSchemaGenerator;
        }

        private static StoreItemCollection LoadStoreItemCollection(
            EntityStoreSchemaGenerator entityStoreSchemaGenerator)
        {
            var xmlReader = RegurgitatingXmlWriter.Regurgitate(
                entityStoreSchemaGenerator.WriteStoreSchema);

            // TODO: do not ignore the errors
            IList<EdmSchemaError> theErrors;
            var storeItemCollection = StoreItemCollection.Create(new[] { xmlReader }, null, null, out theErrors);

            return storeItemCollection;
        }

        private static EntityModelSchemaGenerator CreateEntityModelSchemaGenerator(
            EntityContainer entityContainer)
        {
            var entityModelSchemaGenerator = new EntityModelSchemaGenerator(
                entityContainer,
                "IgnoredNamespace",
                "IgnoredContextName");

            entityModelSchemaGenerator.PluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
            entityModelSchemaGenerator.GenerateForeignKeyProperties = true;
            entityModelSchemaGenerator.GenerateMetadata();

            return entityModelSchemaGenerator;
        }

        private static EdmItemCollection LoadEdmItemCollection(
            EntityModelSchemaGenerator entityModelSchemaGenerator)
        {
            var xmlReader = RegurgitatingXmlWriter.Regurgitate(
                entityModelSchemaGenerator.WriteModelSchema);

            var edmItemCollection = new EdmItemCollection(new[] { xmlReader });
            return edmItemCollection;
        }

        private static EdmMapping LoadEdmMapping(
            EntityModelSchemaGenerator entityModelSchemaGenerator, 
            EdmItemCollection conceptualItemCollection, 
            StoreItemCollection storeItemCollection)
        {
            var xmlReader = RegurgitatingXmlWriter.Regurgitate(
                entityModelSchemaGenerator.WriteStorageMapping);

            var edmMapping = new EdmMapping(
                conceptualItemCollection,
                storeItemCollection,
                xmlReader);

            return edmMapping;
        }

        private static Exception EdmSchemaErrorToException(System.Data.Metadata.Edm.EdmSchemaError error)
        {
            return new Exception();
        }

        private EntityStoreSchemaFilterEntry IgnoreTable(string tableName)
        {
            var filter = new EntityStoreSchemaFilterEntry(
                null, 
                null, 
                tableName, 
                EntityStoreSchemaFilterObjectTypes.Table,
                EntityStoreSchemaFilterEffect.Exclude);

            return filter;
        }
    }
}
