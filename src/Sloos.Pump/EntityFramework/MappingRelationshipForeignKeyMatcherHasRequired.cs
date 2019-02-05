// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingRelationshipForeignKeyMatcherHasRequired : IMappingRelationshipForeignKeyMatcher
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public MappingRelationshipForeignKeyMatcherHasRequired(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public bool IsMatch(NavigationProperty navigationProperty)
        {
            return navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One;
        }

        public string GetEntityName(NavigationProperty navigationProperty)
        {
            return this.codeGenEscaper.Escape(navigationProperty);
        }

        public string GetNecessity()
        {
            return "HasRequired";
        }
    }
}
