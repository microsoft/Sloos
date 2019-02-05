// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Common;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyMatcherIsRowVersion : IMappingPropertyMatcher
    {
        public bool IsMatch(MappingProperty mappingProperty)
        {
            return
                mappingProperty.IsTypeOfByteArray() &&
                mappingProperty.HasFacet(DbProviderManifest.MaxLengthFacetName) &&
                mappingProperty.GetFacet(DbProviderManifest.MaxLengthFacetName).IsUnbounded == false &&
                mappingProperty.GetFacetValue<int>(DbProviderManifest.MaxLengthFacetName) == 8 &&
                mappingProperty.IsStoreGeneratedComputed;
        }
    }
}
