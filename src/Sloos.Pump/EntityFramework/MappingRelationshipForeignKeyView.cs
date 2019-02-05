// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingRelationshipForeignKeyView
    {
        // HasOptional vs. HasRequired
        public string EntityNecessity { get; set; }

        // Product
        public string EntityName { get; set; }

        public List<string> Attributes { get; set; }
    }
}
