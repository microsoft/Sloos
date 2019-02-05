// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;
        private readonly EdmMapping edmMapping;
        private readonly EntityType entityType;

        public MappingViewFactory(
            CodeGenEscaper codeGenEscaper,
            EntityType entityType,
            EdmMapping edmMapping)
        {
            this.codeGenEscaper = codeGenEscaper;
            this.entityType = entityType;
            this.edmMapping = edmMapping;

            this.SetMappingKeyView();
            this.SetMappingPropertyViews();
            this.SetMappingRelationshipForeignKeyViews();
            this.SetMappingManyToManys();
        }

        public MappingKeyView MappingKeyView { get; private set; }
        public MappingPropertyView[] MappingPropertyViews { get; private set; }
        public MappingRelationshipForeignKeyView[] MappingRelationshipForeignKeyViews { get; private set; }
        public MappingManyToManyView[] MappingManyToManysView { get; private set; }

        private void SetMappingManyToManys()
        {
            var factory = new MappingKeyViewFactory(this.entityType);
            this.MappingKeyView = factory.Create();
        }

        private void SetMappingPropertyViews()
        {
            var factory = new MappingPropertyViewFactory(this.codeGenEscaper);
            this.MappingPropertyViews = factory.Create(
                this.edmMapping.EntityMappings[this.entityType].Item1.ElementType).ToArray();
        }

        private void SetMappingRelationshipForeignKeyViews()
        {
            var factory = new MappingRelationshipForeignKeyViewFactory(this.codeGenEscaper);
            this.MappingRelationshipForeignKeyViews = factory.Create(this.entityType).ToArray();
        }

        private void SetMappingKeyView()
        {
            var factory = new MappingManyToManyViewFactory(this.edmMapping);
            this.MappingManyToManysView = factory.Create(this.entityType).ToArray();
        }
    }
}
