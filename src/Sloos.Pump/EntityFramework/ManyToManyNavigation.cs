// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class ManyToManyNavigation
    {
        public ManyToManyNavigation(NavigationProperty hasManyNavigationProperty)
        {
            this.HasMany = hasManyNavigationProperty;
            
            this.WithMany = this.HasMany
                .ToEndMember
                .GetEntityType()
                .NavigationProperties
                .Single(x => x.RelationshipType == hasManyNavigationProperty.RelationshipType && x != hasManyNavigationProperty);
        }

        public NavigationProperty HasMany { get; }
        public EntityType HasManyEntity => (EntityType)this.HasMany.DeclaringType;
        public RelationshipEndMember HasManyEndMember => this.HasMany.FromEndMember;

        public NavigationProperty WithMany { get; }
        public EntityType WithManyEntity => (EntityType)this.WithMany.DeclaringType;
        public RelationshipEndMember WithManyEndMember => this.WithMany.FromEndMember;
    }
}
