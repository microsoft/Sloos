// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingManyToManyView
    {
        private readonly ManyToManyKeys manyToManyKeys;

        private readonly ManyToManyNavigation manyToManyNavigation;
        private readonly ManyToManyStoreTable manyToManyStoreTable;

        public MappingManyToManyView(
            ManyToManyNavigation manyToManyNavigation,
            ManyToManyStoreTable manyToManyStoreTable,
            ManyToManyKeys manyToManyKeys)
        {
            this.manyToManyNavigation = manyToManyNavigation;
            this.manyToManyStoreTable = manyToManyStoreTable;
            this.manyToManyKeys = manyToManyKeys;
        }

        public string HasMany => this.manyToManyNavigation.HasMany.Name;
        public string WithMany => this.manyToManyNavigation.WithMany.Name;
        public string MappingTableName => this.manyToManyStoreTable.MappingTableName;
        public string MappingSchemaName => this.manyToManyStoreTable.MappingSchemaName;
        public string[] MapLeftKeys => this.manyToManyKeys.MapLeftKeys;
        public string[] MapRightKeys => this.manyToManyKeys.MapRightKeys;
    }
}
