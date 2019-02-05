// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text;
using Sloos.Pump.EntityFramework;

namespace Sloos.Pump.TextTemplate
{
    public sealed class MappingFactory : IFactory
    {
        private readonly EntityName entityName;
        private readonly EntityNamespace entityNamespace;
        private readonly MappingViewFactory mappingViewFactory;

        public MappingFactory(
            EntityName entityName,
            EntityNamespace entityNamespace,
            MappingViewFactory mappingViewFactory)
        {
            this.entityName = entityName;
            this.entityNamespace = entityNamespace;
            this.mappingViewFactory = mappingViewFactory;
        }

        public string ConceptualMappingName => this.entityName.ConceptualMappingName;
        public string ConceptualName => this.entityName.ConceptualName;
        public string StoreName => this.entityName.StoreName;
        public string Namespace => this.entityNamespace.MappingNamespace;
        public string SchemaName => "dbo";

        public MappingKeyView MappingKeyView => this.mappingViewFactory.MappingKeyView;
        public MappingPropertyView[] MappingPropertyViews => this.mappingViewFactory.MappingPropertyViews;
        public MappingRelationshipForeignKeyView[] MappingRelationshipForeignKeyViews => this.mappingViewFactory.MappingRelationshipForeignKeyViews;
        public MappingManyToManyView[] MappingManyToManyViews => this.mappingViewFactory.MappingManyToManysView;

        public void WriteTo(StringBuilder sb)
        {
            var template = new Mapping()
            {
                Factory = this,
            };

            sb.AppendLine(
                template.TransformText());
        }
    }
}
