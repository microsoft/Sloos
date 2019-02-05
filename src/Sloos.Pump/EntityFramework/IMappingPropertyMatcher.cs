// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Pump.EntityFramework
{
    public interface IMappingPropertyMatcher
    {
        bool IsMatch(MappingProperty mappingProperty);
    }
}
