// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Sloos.Pump.EntityFramework
{
    public sealed class ContextViewFactory
    {
        private readonly string name;
        private readonly EntityNamespace entityNamespace;
        private readonly IEnumerable<EntityType> entityTypes;
        private readonly ContextEntityViewFactory factory;

        public ContextViewFactory(string name, EntityNamespace entityNamespace, IEnumerable<EntityType> entityTypes)
        {
            this.name = name;
            this.entityNamespace = entityNamespace;
            this.entityTypes = entityTypes;

            this.factory = new ContextEntityViewFactory();
        }

        public ContextView Create()
        {
            var view = new ContextView()
                           {
                               Name = this.name,
                               ModelNamespace = this.entityNamespace.ModelNamespace,
                               ContextEntityViews = this.entityTypes.Select(x => this.factory.Create(x)).ToArray(),
                           };

            return view;
        }
    }
}
