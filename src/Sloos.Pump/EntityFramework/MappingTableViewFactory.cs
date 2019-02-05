// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingTableViewFactory
    {
        public MappingTableView Create(EntityType entityType)
        {
            var mappingTableView = new MappingTableView()
                                       {
                                           Schema = (string)entityType.MetadataProperties["Schema"].Value,
                                           Table = (string)entityType.MetadataProperties["Table"].Value,
                                       };

            return mappingTableView;
        }
    }
}
