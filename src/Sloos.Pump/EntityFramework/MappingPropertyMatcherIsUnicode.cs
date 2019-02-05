// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Common;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyMatcherIsUnicode : IMappingPropertyMatcher
    {
        public bool IsMatch(MappingProperty mappingProperty)
        {
            return
                mappingProperty.IsTypeOfString() &&
                mappingProperty.HasFacet(DbProviderManifest.UnicodeFacetName) &&
                mappingProperty.GetFacetValue<bool>(DbProviderManifest.UnicodeFacetName);
        }
    }
}
