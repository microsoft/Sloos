// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;

namespace Sloos.Pump.EntityFramework
{
    public interface IGeneratorFactory
    {
        StoreItemCollection StoreItemCollection { get; }
        EdmItemCollection ConceptualItemCollection { get; }
        EdmMapping EdmMapping { get; }
    }
}
