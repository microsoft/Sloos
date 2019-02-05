// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Pump.Test.Builder
{
    public interface IEntityFrameworkBuilder<T> 
        where T : class
    {
        T Build();
    }
}
