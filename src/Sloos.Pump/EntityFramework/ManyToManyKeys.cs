// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class ManyToManyKeys
    {
        public ManyToManyKeys(
            EntityType leftType, Dictionary<EdmMember, string> leftKeyMappings,
            EntityType rightType, Dictionary<EdmMember, string> rightKeyMappings) 
        {
            this.MapLeftKeys = leftType.KeyMembers.Select(x => leftKeyMappings[x]).ToArray();
            this.MapRightKeys = rightType.KeyMembers.Select(x => rightKeyMappings[x]).ToArray();
        }

        public string[] MapLeftKeys { get; }
        public string[] MapRightKeys { get; }
    }
}
