// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingRelationshipForeignKeyMatcherWithMany : IMappingRelationshipForeignKeyMatcher
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public MappingRelationshipForeignKeyMatcherWithMany(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public bool IsMatch(NavigationProperty navigationProperty)
        {
            return navigationProperty.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many;
        }

        public string GetEntityName(NavigationProperty navigationProperty)
        {
            return this.codeGenEscaper.Escape(navigationProperty);
        }
    }
}
