// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Sloos.Pump.EntityFramework
{
    public interface IMappingRelationshipForeignKeyMatcher
    {
        bool IsMatch(NavigationProperty navigationProperty);
    }
}
