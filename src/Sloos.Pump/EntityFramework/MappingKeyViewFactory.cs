// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingKeyViewFactory  
    {
        private readonly EntityType entityType;

        public MappingKeyViewFactory(EntityType entityType)
        {
            this.entityType = entityType;
        }

        public MappingKeyView Create()
        {
            var keys = this.entityType.KeyProperties.Select(x => x.Name);
            return new MappingKeyView(keys);
        }
    }
}
