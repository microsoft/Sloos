// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class EntityNavigationViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public EntityNavigationViewFactory(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public IEnumerable<EntityNavigationView> Create(EntityType entityType)
        {
            return entityType
                .NavigationProperties
                .Select(this.Create);
        }

        private EntityNavigationView Create(NavigationProperty property)
        {
            var view = new EntityNavigationView()
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

            return this.IsManyMultiplicity(endMember.RelationshipMultiplicity)
                       ? this.codeGenEscaper.EscapeCollection(endMember.GetEntityType().Name)
                       : endMember.GetEntityType().Name;
        }

        private bool IsManyMultiplicity(RelationshipMultiplicity multiplicity)
        {
            return multiplicity == RelationshipMultiplicity.Many;
        }
    }
}
