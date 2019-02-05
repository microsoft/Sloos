// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class EntityEntityViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public EntityEntityViewFactory(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public IEnumerable<EntityEntityView> Create(EntityType entityType)
        {
            return entityType.Properties.Select(this.Create);
        }

        private EntityEntityView Create(EdmProperty property)
        {
            var view = new EntityEntityView()
                           {
                               Name = this.codeGenEscaper.Escape(property.Name),
                               TypeName = this.codeGenEscaper.Escape(property.TypeUsage),
                           };

            return view;
        }
    }
}
