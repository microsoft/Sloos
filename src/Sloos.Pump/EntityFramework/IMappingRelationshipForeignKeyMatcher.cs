// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;

namespace Sloos.Pump.EntityFramework
{
    public interface IMappingRelationshipForeignKeyMatcher
    {
        bool IsMatch(NavigationProperty navigationProperty);
    }
}
