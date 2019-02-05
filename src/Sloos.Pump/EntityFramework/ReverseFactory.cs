// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using Sloos.Pump.TextTemplate;

namespace Sloos.Pump.EntityFramework
{
    public sealed class ReverseFactory : IFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;

        private readonly EntityNamespace entityNamespace;
        private readonly string contextName;
        private readonly IGeneratorFactory generatorFactory;

        public ReverseFactory(
            IGeneratorFactory generatorFactory,
            EntityNamespace entityNamespace,
            string contextName)
        {
            this.generatorFactory = generatorFactory;
            this.entityNamespace = entityNamespace;
            this.contextName = contextName;

            this.codeGenEscaper = new CodeGenEscaper();

            this.ConceptualEntityTypes = this.generatorFactory.ConceptualItemCollection
                .OfType<EntityContainer>()
                .Single()
                .BaseEntitySets
                .OfType<EntitySet>()
                .Select(x => x.ElementType)
                .ToArray();

            this.StoreEntityTypes = this.ConceptualEntityTypes
                .Select(x => this.generatorFactory.EdmMapping.EntityMappings[x].Item1.ElementType)
                .ToArray();
        }

        public EntityType[] ConceptualEntityTypes { get; }
        public EntityType[] StoreEntityTypes { get; }

        public void WriteTo(StringBuilder sb)
        {
            this.ContextWriteTo(sb);
            this.EntityWriteTo(sb);
            this.MappingWriteTo(sb);
        }

        private void ContextWriteTo(StringBuilder sb)
        {
            var contextViewFactory = new ContextViewFactory(
                this.contextName,
                this.entityNamespace,
                this.StoreEntityTypes);

            var contextFactory = new ContextFactory(
                contextViewFactory.Create(),
                this.entityNamespace);

            contextFactory.WriteTo(sb);
        }
        
        private void EntityWriteTo(StringBuilder sb)
        {
            var factories = this.ConceptualEntityTypes
                .Select(x => new
                {
                    EntityName = new EntityName(x.Name),
                    Factory = new EntityViewFactory(this.codeGenEscaper, x),
                })
                .Select(x => new EntityFactory(x.EntityName, this.entityNamespace, x.Factory));

            foreach (var factory in factories)
            {
                factory.WriteTo(sb);
            }
        }

        private void MappingWriteTo(StringBuilder sb)
        {
            var factories = this.ConceptualEntityTypes
                .Select(x => new
                {
                    EntityName = new EntityName(x.Name),
                    Factory = new MappingViewFactory(this.codeGenEscaper, x, this.generatorFactory.EdmMapping),
                })
                .Select(x => new MappingFactory(x.EntityName, this.entityNamespace, x.Factory));

            foreach (var factory in factories)
            {
                factory.WriteTo(sb);
            }
        }
    }
}
