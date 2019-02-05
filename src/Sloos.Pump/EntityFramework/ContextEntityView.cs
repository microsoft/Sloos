// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Pump.EntityFramework
{
    public sealed class ContextEntityView
    {
        private readonly EntityName entityName;

        public ContextEntityView(EntityName entityName)
        {
            this.entityName = entityName;
        }

        public string StoreName => this.entityName.StoreName;
        public string ConceptualName => this.entityName.ConceptualName;
        public string ConceptualMappingName => this.entityName.ConceptualMappingName;
    }
}
