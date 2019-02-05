// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text;
using Sloos.Pump.EntityFramework;

namespace Sloos.Pump.TextTemplate
{
    public sealed class EntityFactory : IFactory
    {
        private readonly EntityName entityName;
        private readonly EntityNamespace entityNamespace;
        private readonly EntityViewFactory entityViewFactory;

        public EntityFactory(
            EntityName entityName,
            EntityNamespace entityNamespace,
            EntityViewFactory entityViewFactory)
        {
            this.entityName = entityName;
            this.entityNamespace = entityNamespace;
            this.entityViewFactory = entityViewFactory;
        }

        public string ConceptualName => this.entityName.ConceptualName;
        public string StoreName => this.entityName.StoreName;
        public string Namespace => this.entityNamespace.ModelNamespace;

        public EntityConstructorView[] EntityConstructorViews => this.entityViewFactory.EntityConstructorViews;
        public EntityEntityView[] Properties => this.entityViewFactory.EntityEntityViews;
        public EntityNavigationView[] NavigationProperties => this.entityViewFactory.EntityNavigationViews;

        public void WriteTo(StringBuilder sb)
        {
            var template = new Entity
            {
                Factory = this,
            };

            sb.AppendLine(
                template.TransformText());
        }
    }
}
