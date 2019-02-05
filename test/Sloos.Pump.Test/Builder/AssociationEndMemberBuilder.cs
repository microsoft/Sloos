// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.Test.Builder
{
    public class AssociationEndMemberBuilder : IEntityFrameworkBuilder<AssociationEndMember>
    {
        private OperationAction deleteAction;

        private EntityType entityType;
        private RelationshipMultiplicity relationshipMultiplicity;

        public AssociationEndMember Build()
        {
            var associationEndMember = AssociationEndMember.Create(
                this.entityType.Name,
                this.entityType.GetReferenceType(),
                this.relationshipMultiplicity,
                this.deleteAction,
                Enumerable.Empty<MetadataProperty>());

            return associationEndMember;
        }

        public AssociationEndMemberBuilder WithEntityType(EntityType entityType)
        {
            this.entityType = entityType;
            return this;
        }

        public AssociationEndMemberBuilder RelationshipMultiplicity(RelationshipMultiplicity relationshipMultiplicity)
        {
            this.relationshipMultiplicity = relationshipMultiplicity;
            return this;
        }

        public AssociationEndMemberBuilder DeleteAction(OperationAction deleteAction)
        {
            this.deleteAction = deleteAction;
            return this;
        }
    }
}
