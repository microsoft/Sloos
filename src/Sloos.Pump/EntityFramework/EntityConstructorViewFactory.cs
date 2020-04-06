// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Sloos.Pump.EntityFramework
{
    public sealed class EntityConstructorViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public EntityConstructorViewFactory(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public IEnumerable<EntityConstructorView> Create(EntityType entityType)
        {
            return entityType
                .NavigationProperties
                .Where(x => x.DeclaringType == entityType)
                .Where(x => this.IsManyMultiplicity(x.ToEndMember.RelationshipMultiplicity))
                .Select(this.Create);
        }

        private EntityConstructorView Create(NavigationProperty property)
        {
            var view = new EntityConstructorView()
                           {
                               Name = this.codeGenEscaper.Escape(property.Name),
                               TypeName = this.GetTypeName(property.ToEndMember)
                           };

            return view;
        }

        private string GetTypeName(RelationshipEndMember endMember)
        {
            // This type is not fully qualified (escaped) because it is part of 
            // namespace of the generated code, and there's no need to fully 
            // qualify it.
            return this.codeGenEscaper.EscapeList(endMember.GetEntityType().Name);
        }

        private bool IsManyMultiplicity(RelationshipMultiplicity multiplicity)
        {
            return multiplicity == RelationshipMultiplicity.Many;
        }
    }
}
