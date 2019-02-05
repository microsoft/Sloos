// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingRelationshipForeignKeyMatcherHasForeignKey : IMappingRelationshipForeignKeyMatcher
    {
        private readonly List<string> foreignKeys;
        private readonly KeyFormatter keyFormatter;

        public MappingRelationshipForeignKeyMatcherHasForeignKey()
        {
            this.foreignKeys = new List<string>();
            this.keyFormatter = new KeyFormatter();
        }

        public IEnumerable<string> ForeignKeys => this.foreignKeys;

        public bool IsMatch(NavigationProperty navigationProperty)
        {
            bool isMatch = navigationProperty.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many;

            if (isMatch)
            {
                this.foreignKeys.AddRange(
                    this.GetForeignKeys(navigationProperty));
            }

            return isMatch;
        }

        private IEnumerable<string> GetForeignKeys(NavigationProperty navigationProperty)
        {
            var associations = (AssociationType)navigationProperty.RelationshipType;
            return associations.ReferentialConstraints
                .Single()
                .ToProperties
                .Select(x => x.Name);
        }

        public string FormattedKeys(NavigationProperty navigationProperty)
        {
            return this.keyFormatter.Format(this.GetForeignKeys(navigationProperty));
        }
    }
}
