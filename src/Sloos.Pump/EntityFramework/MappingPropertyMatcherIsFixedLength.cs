// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Common;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyMatcherIsFixedLength : IMappingPropertyMatcher
    {
        public bool IsMatch(MappingProperty mappingProperty)
        {
            return
                (mappingProperty.IsTypeOfString() || mappingProperty.IsTypeOfByteArray()) &&
                mappingProperty.HasFacet(DbProviderManifest.FixedLengthFacetName) &&
                mappingProperty.GetFacetValue<bool>(DbProviderManifest.FixedLengthFacetName);
        }
    }
}
