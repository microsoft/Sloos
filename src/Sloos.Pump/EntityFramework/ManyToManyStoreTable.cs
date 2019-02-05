// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;

namespace Sloos.Pump.EntityFramework
{
    public sealed class ManyToManyStoreTable
    {
        public ManyToManyStoreTable(EntitySet entitySet)
        {
            this.MappingTableName  = (string)entitySet.MetadataProperties["Table"].Value ?? entitySet.Name;
            this.MappingSchemaName = (string)entitySet.MetadataProperties["Schema"].Value;
        }

        public string MappingTableName { get; }
        public string MappingSchemaName { get; }
    }
}
