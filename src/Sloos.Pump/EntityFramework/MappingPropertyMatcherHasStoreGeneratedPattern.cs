// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyMatcherHasStoreGeneratedPattern : IMappingPropertyMatcher
    {
        private readonly EntityType entityType;

        public MappingPropertyMatcherHasStoreGeneratedPattern(EntityType entityType)
        {
            this.entityType = entityType;
        }

        public bool IsMatch(MappingProperty mappingProperty)
        {
            return
                mappingProperty.IsTypeOfNumericKey() &&
                (this.IsStoreGeneratedPatternNone(mappingProperty) || this.IsStoreGeneratedPatternIdentity(mappingProperty));
        }

        public StoreGeneratedPattern GetStoreGeneratedPattern(MappingProperty mappingProperty)
        {
            return (this.IsStoreGeneratedPatternNone(mappingProperty)) ?
                       StoreGeneratedPattern.None :
                       StoreGeneratedPattern.Identity;
        }

        private bool IsStoreGeneratedPatternNone(MappingProperty mappingProperty)
        {
            return
                this.IsKey(mappingProperty) &&
                !mappingProperty.IsStoreGeneratedIdentity;
        }

        private bool IsStoreGeneratedPatternIdentity(MappingProperty mappingProperty)
        {
            return
                (!this.IsKey(mappingProperty) || this.HasMultipleKeys()) &&
                mappingProperty.IsStoreGeneratedIdentity;
        }

        private bool HasMultipleKeys()
        {
            return this.entityType.KeyProperties.Count > 1;
        }

        private bool IsKey(MappingProperty mappingProperty)
        {
            return this.entityType.KeyProperties.Any(x => x.Name == mappingProperty.Name);
        }
    }
}
