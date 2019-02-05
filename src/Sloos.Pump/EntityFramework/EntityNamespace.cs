// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Sloos.Pump.EntityFramework
{
    public sealed class EntityNamespace
    {
        public EntityNamespace(string nameSpace)
        {
            this.Namespace = nameSpace;
        }

        public string Namespace { get; }
        public string MappingNamespace => $"{this.Namespace}.Models.Mapping";
        public string ModelNamespace => $"{this.Namespace}.Models";
    }
}
