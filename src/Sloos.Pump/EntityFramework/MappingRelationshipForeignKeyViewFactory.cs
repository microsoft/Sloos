// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingRelationshipForeignKeyViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public MappingRelationshipForeignKeyViewFactory(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public IEnumerable<MappingRelationshipForeignKeyView> Create(EntityType entityType)
        {
            return entityType
                .NavigationProperties
                .Where(x => x.DeclaringType == entityType && this.IsForeignKey(x) && this.HasReferentialConstraint(x))
                .Select(this.CreateForeignKeyRelationship)
                .Select(this.CreateView);
        }

        private MappingRelationshipForeignKeyView CreateView(ForeignKeyRelationship relationship)
        {
            var view = new MappingRelationshipForeignKeyView
                           {
                               EntityNecessity = this.GetEntityNecessity(relationship.Source),
                               EntityName = this.GetEntityName(relationship.Source),
                               Attributes = this.CreateAttributes(relationship).ToList(),
                           };

            return view;
        }

        private IEnumerable<string> CreateAttributes(ForeignKeyRelationship relationship)
        {
            var withManyMatcher = new MappingRelationshipForeignKeyMatcherWithMany(this.codeGenEscaper);
            var isWithManyMatch = withManyMatcher.IsMatch(relationship.Source);

            if (isWithManyMatch)
            {
                yield return $".WithMany(x => x.{withManyMatcher.GetEntityName(relationship.Target)})";
            }

            var hasForeignKeyMatcher = new MappingRelationshipForeignKeyMatcherHasForeignKey();
            if (isWithManyMatch && hasForeignKeyMatcher.IsMatch(relationship.Source))
            {
                yield return $".HasForeignKey({hasForeignKeyMatcher.FormattedKeys(relationship.Source)})";
            }

            var withOptionalMatcher = new MappingRelationshipForeignKeyMatcherWithOptional(this.codeGenEscaper);
            if (withOptionalMatcher.IsMatch(relationship.Source))
            {
                yield return $".WithOptional(x => x.{withOptionalMatcher.GetEntityName(relationship.Target)})";
            }
        }

        private string GetEntityName(NavigationProperty navigationProperty)
        {
            string entityName = null;

            var requiredMatcher = new MappingRelationshipForeignKeyMatcherHasRequired(this.codeGenEscaper);
            if (requiredMatcher.IsMatch(navigationProperty))
            {
                entityName = requiredMatcher.GetEntityName(navigationProperty);
            }

            var optionalMatcher = new MappingRelationshipForeignKeyMatcherHasOptional(this.codeGenEscaper);
            if (optionalMatcher.IsMatch(navigationProperty))
            {
                entityName = optionalMatcher.GetEntityName(navigationProperty);
            }

            return entityName;
        }
        
        private string GetEntityNecessity(NavigationProperty navigationProperty)
        {
            string entityNecessity = null;

            var requiredMatcher = new MappingRelationshipForeignKeyMatcherHasRequired(this.codeGenEscaper);
            if (requiredMatcher.IsMatch(navigationProperty))
            {
                entityNecessity = requiredMatcher.GetNecessity();
            }

            var optionalMatcher = new MappingRelationshipForeignKeyMatcherHasOptional(this.codeGenEscaper);
            if (optionalMatcher.IsMatch(navigationProperty))
            {
                entityNecessity = optionalMatcher.GetNecessity();
            }

            return entityNecessity;
        }

        private ForeignKeyRelationship CreateForeignKeyRelationship(NavigationProperty navigationProperty)
        {
            var relationship = new ForeignKeyRelationship()
                                   {
                                       Source = navigationProperty,
                                       Target = this.GetOtherNavigationProperty(navigationProperty),
                                   };

            return relationship;
        }

        private NavigationProperty GetOtherNavigationProperty(NavigationProperty navigationProperty)
        {
            var other = navigationProperty
                .ToEndMember
                .GetEntityType()
                .NavigationProperties.Single(n => n.RelationshipType == navigationProperty.RelationshipType && n != navigationProperty);

            return other;
        }

        private bool IsForeignKey(NavigationProperty navigationProperty)
        {
            var association = (AssociationType)navigationProperty.RelationshipType;
            return association.IsForeignKey;
        }

        private bool HasReferentialConstraint(NavigationProperty navigationProperty)
        {
            var association = (AssociationType)navigationProperty.RelationshipType;
            bool hasReferentialConstraint = association.ReferentialConstraints.Single().ToRole == navigationProperty.FromEndMember;
            return hasReferentialConstraint;
        }

        private sealed class ForeignKeyRelationship
        {
            public NavigationProperty Source { get; set; }
            public NavigationProperty Target { get; set; }
        }
    }
}
