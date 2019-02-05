// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingManyToManyViewFactory
    {
        private readonly EdmMapping edmMapping;

        public MappingManyToManyViewFactory(EdmMapping edmMapping)
        {
            this.edmMapping = edmMapping;
        }

        public IEnumerable<MappingManyToManyView> Create(EntityType entityType)
        {
            return entityType
                .NavigationProperties
                .Where(x =>
                       x.DeclaringType == entityType &&
                       x.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many &&
                       x.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many &&
                       x.RelationshipType.RelationshipEndMembers.First() == x.FromEndMember)
                .Select(this.Create);
        }

        private MappingManyToManyView Create(NavigationProperty navigationProperty)
        {
            var manyToManyNavigation = new ManyToManyNavigation(navigationProperty);

            var mapping = this.GetStoreMapping((AssociationType)navigationProperty.RelationshipType);
            var manyToManyStoreTable = new ManyToManyStoreTable(mapping.Item1);
            var manyToManyKeys = new ManyToManyKeys(
                manyToManyNavigation.HasManyEntity, mapping.Item2[manyToManyNavigation.HasManyEndMember],
                manyToManyNavigation.WithManyEntity, mapping.Item2[manyToManyNavigation.WithManyEndMember]);

            var mappingManyToMany = new MappingManyToManyView(manyToManyNavigation, manyToManyStoreTable, manyToManyKeys);
            return mappingManyToMany;
        }

        private Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>> GetStoreMapping(AssociationType associationType)
        {
            var mapping = this.edmMapping.ManyToManyMappings[associationType];
            return mapping;
        }
    }
}
