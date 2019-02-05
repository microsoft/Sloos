// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingKeyView
    {
        private readonly string[] keys;

        public MappingKeyView(IEnumerable<string> keys)
        {
            this.keys = keys.ToArray();
        }

        public IEnumerable<string> Keys => this.keys;

        // TODO: why is this code doing AsStatement()?  It should all be done in the .tt file.
        // TODO: why is this code not using KeyFormatter?
        public string AsStatement()
        {
            switch (this.keys.Length)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return $"this.HasKey(x => x.{this.keys[0]});";
                default:
                    var formattedKeys = this.Keys.Select(x => $"x.{x}");
                    var joinedKeys = string.Join(", ", formattedKeys);
                    return $"this.HasKey(x => new {{ {joinedKeys} }});";
            }
        }
    }
}
