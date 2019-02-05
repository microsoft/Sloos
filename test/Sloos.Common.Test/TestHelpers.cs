// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.IO;
using System.Text;

namespace Sloos.Common.Test
{
    public static class TestHelpers
    {
        public static Stream StringArrayToStream(string[] array)
        {
            var row = string.Join(",", array);
            return new MemoryStream(Encoding.UTF8.GetBytes(row));
        }
    }
}
