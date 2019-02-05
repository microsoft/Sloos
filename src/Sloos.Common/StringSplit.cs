// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Sloos.Common
{
    public static class StringSplit
    {
        public static string AtIndex(string s, string separator, int index)
        {
            int offset = 0;
            while (index-- > 0)
            {
                offset = s.IndexOf(separator, offset, System.StringComparison.Ordinal);
                if (offset < 0 && index == 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                offset += separator.Length;
            }

            if (offset < 0 || offset == s.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            int endOffset = s.IndexOf(separator, offset, System.StringComparison.Ordinal);
            if (endOffset < 0)
            {
                return s.Substring(offset);
            }

            return s.Substring(offset, endOffset - offset);
        }
    }
}
