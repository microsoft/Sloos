// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class KeyFormatter
    {
        public string Format(params string[] keys)
        {
            return this.Format((IEnumerable<string>)keys);
        }

        public string Format(IEnumerable<string> keys)
        {
            if (keys.Count() == 1)
            {
                return $"x => x.{keys.First()}";
            }

            var joinedKeys = keys.Select(x => $"x.{x}");
            var joinedKeyStrings = string.Join(", ", joinedKeys);

            return $"x => new {{ {joinedKeyStrings} }}";
        }
    }
}
