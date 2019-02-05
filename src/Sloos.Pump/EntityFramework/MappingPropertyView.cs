// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyView
    {
        public MappingPropertyView()
        {
            this.Attributes = new List<string>();
        }

        public string ConceptualName { get; set; }
        public string StoreName { get; set; }

        public List<string> Attributes { get; }
    }
}
