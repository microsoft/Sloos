// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


namespace Sloos.Pump.EntityFramework
{
    public sealed class ContextView    
    {
        public string Name { get; set; }
        public string ModelNamespace { get; set; }
        public ContextEntityView[] ContextEntityViews { get; set; }
    }
}
