// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingColumnNameViewFactory
    {
        private readonly EdmMapping edmMapping;

        public MappingColumnNameViewFactory(EdmMapping edmMapping)
        {
            this.edmMapping = edmMapping;
        }

        public IEnumerable<MappingColumnNameView> Create(EntityType entityType)
        {
            var conceptualToStoreMap = this.edmMapping.EntityMappings[entityType].Item2;
            // Iterate over entityType.Properties to maintain the original ordering.
            return entityType.Properties.Select(x => this.Create(conceptualToStoreMap, x));
        }

        private MappingColumnNameView Create(
            Dictionary<EdmProperty, EdmProperty> conceptualToStoreMap, 
            EdmProperty edmProperty)
        {
            var storeName = conceptualToStoreMap[edmProperty].Name;

            var mappingColumnNameView = new MappingColumnNameView()
                                            {
                                                ConceptualName = edmProperty.Name,
                                                StoreName = storeName,
                                            };

            return mappingColumnNameView;
        }
    }
}
