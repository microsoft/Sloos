// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyMatcherIsRequired : IMappingPropertyMatcher
    {
        public bool IsMatch(MappingProperty mappingProperty)
        {
            return
                (mappingProperty.IsTypeOfString() || mappingProperty.IsTypeOfByteArray()) &&
                !mappingProperty.IsNullable;
        }
    }
}
