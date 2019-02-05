// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Common;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyMatcherMaxLength : IMappingPropertyMatcher
    {
        public bool IsMatch(MappingProperty mappingProperty)
        {
            return
                (mappingProperty.IsTypeOfString() || mappingProperty.IsTypeOfByteArray()) &&
                mappingProperty.HasFacet(DbProviderManifest.MaxLengthFacetName) &&
                !mappingProperty.GetFacet(DbProviderManifest.MaxLengthFacetName).IsUnbounded &&
                // XXX: I (currently) have no clue how to write a simple test for this.  For some
                // reason a varchar(max) or nvarchar(max) did not have IsUnbounded set, so this 
                // match did not fire, and thereby caused the code to emit a superfluous 
                // .HasMaxLength(2^31) or .HasMaxLength(2^30) respectively.  The hack (below) is
                // my current work around, but I think it is stupid.
                !this.IsMaxType(mappingProperty.TypeName);
        }

        public int GetMaxLength(MappingProperty mappingProperty)
        {
            return mappingProperty.GetFacetValue<int>(DbProviderManifest.MaxLengthFacetName);
        }

        private bool IsMaxType(string typeName)
        {
            return typeName.EndsWith("varchar(max)") || typeName == "varbinary(max)";
        }
    }
}
