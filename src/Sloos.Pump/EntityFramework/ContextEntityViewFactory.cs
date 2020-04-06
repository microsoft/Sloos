// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Sloos.Pump.EntityFramework
{
    public sealed class ContextEntityViewFactory
    {
        public ContextEntityView Create(EntityType entityType)
        {
            return new ContextEntityView(
                new EntityName(entityType.Name));
        }
    }
}
