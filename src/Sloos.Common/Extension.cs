// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sloos
{
    public static class Extension
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) 
        {
            return source.SelectMany(x => x);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            return source.Concat(new[] { item });
        }

        public static Stream ToStream(this string source)
        {
            return new MemoryStream(
                Encoding.UTF8.GetBytes(source));
        }

        public static Stream ToStream(this StringBuilder source)
        {
            return source.ToString().ToStream();
        }
    }
}
