// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Sloos.Common
{
    public sealed class ElectedColumnType
    {
        public Type TypeOfAllRows { get; set; }
        public Type TypeOfNonHeaderRows { get; set; }
    }
}
