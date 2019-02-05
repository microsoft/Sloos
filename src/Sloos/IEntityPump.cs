// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Spike
{
    public interface IEntityPump : IDisposable
    {
        void CreateIfNotExists();
    }
}
