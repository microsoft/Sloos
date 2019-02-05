// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Text;
using Xunit;

namespace Sloos.Common.Test
{
    public static class Extension
    {
        public static Exception ShouldThrow<T>(this Action action)
        {
            Exception exception = null;

            try
            {
                action.Invoke();
                Assert.False(true, "Did not expect to execute this statement!");
            }
            catch (Exception e)
            {
                Assert.IsType<T>(e);
                exception = e;
            }

            return exception;
        }

        public static Stream ToStream(this string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s));
        }
    }
}
