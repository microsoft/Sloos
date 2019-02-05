// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Text;

namespace Sloos
{
    public static class ArgsHelpers
    {
        public static string PropertyToOptionName(string propertyName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in propertyName.ToCharArray())
            {
                if (Char.IsUpper(c))
                {
                    sb.Append('-');
                    sb.Append(Char.ToLower(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().TrimStart('-');
        }
    }
}
