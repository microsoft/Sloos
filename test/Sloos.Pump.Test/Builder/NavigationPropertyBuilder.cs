// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.Test.Builder
{
    public class NavigationPropertyBuilder : IEntityFrameworkBuilder<NavigationProperty>
    {
        private readonly AssociationEndMemberBuilder builder = new AssociationEndMemberBuilder();

        private AssociationType associationType;

        private AssociationEndMember fromAssociationEndMember;

        private EntityType fromEntityType;

        private string[] fromPropertyNames;

        private AssociationEndMember toAssociationEndMember;

        private EntityType toEntityType;

        private string[] toPropertyNames;

        public AssociationType RelationshipType => this.associationType;

        public NavigationProperty Build()
        {
            var referentialConstraint = new ReferentialConstraint(
                this.fromAssociationEndMember,
                this.toAssociationEndMember,
                this.GetKeyProperties(this.fromPropertyNames, this.fromEntityType.Properties),
                this.GetKeyProperties(this.toPropertyNames, this.toEntityType.Properties));

            var relationshipType = this.associationType ?? this.CreateAssociationType(referentialConstraint);

            var navigationProperty = NavigationProperty.Create(
                this.toEntityType.Name,
                this.toAssociationEndMember.TypeUsage,
                relationshipType,
                this.fromAssociationEndMember,
                this.toAssociationEndMember,
                Enumerable.Empty<MetadataProperty>());

            // XXX: this class is not thread safe, but we can still clear
            // the debris from last time.
            this.associationType = null;

            return navigationProperty;
        }

        private AssociationType CreateAssociationType(ReferentialConstraint referentialConstraint)
        {
            var relationshipName = $"{this.fromEntityType.Name}_{this.toEntityType.Name}";

            var relationshipType = AssociationType.Create(
                relationshipName,
                this.fromEntityType.NamespaceName,
                // NOTE: I am blindly assuming this is true.  I think this is a 
                // safe bet for my tests.
                true/*foreignKey*/,
                DataSpace.CSpace,
                this.fromAssociationEndMember,
                this.toAssociationEndMember,
                referentialConstraint,
                Enumerable.Empty<MetadataProperty>());

            return relationshipType;
        }

        public NavigationPropertyBuilder FromRole(
            EntityType entityType, 
            RelationshipMultiplicity relationshipMultiplicity, 
            OperationAction deleteAction = OperationAction.None)
        {
            this.fromEntityType = entityType;
            this.fromAssociationEndMember = this.builder
                .WithEntityType(entityType)
                .RelationshipMultiplicity(relationshipMultiplicity)
                .DeleteAction(deleteAction)
                .Build();
            
            return this;
        }

        public NavigationPropertyBuilder ToRole(
            EntityType entityType, 
            RelationshipMultiplicity relationshipMultiplicity, 
            OperationAction deleteAction = OperationAction.None)
        {
            this.toEntityType = entityType;
            this.toAssociationEndMember = this.builder
                .WithEntityType(entityType)
                .RelationshipMultiplicity(relationshipMultiplicity)
                .DeleteAction(deleteAction)
                .Build();

            return this;
        }

        public NavigationPropertyBuilder Map(string from, string to)
        {
            this.fromPropertyNames = new[] { from };
            this.toPropertyNames = new[] { to };

            return this;
        }

        public NavigationPropertyBuilder Map(string[] from, string[] to)
        {
            this.fromPropertyNames = from;
            this.toPropertyNames = to;

            return this;
        }

        public NavigationPropertyBuilder WithSameRelationshipAs(NavigationProperty otherNavigationProperty)
        {
            this.associationType = (AssociationType)otherNavigationProperty.RelationshipType;
            return this;
        }

        private IEnumerable<EdmProperty> GetKeyProperties(
            IEnumerable<string> keys,
            IEnumerable<EdmProperty> properties)
        {
            var keyProperties = from key in keys
                                join property in properties on key equals property.Name
                                select property;

            return keyProperties;
        }
    }
}
