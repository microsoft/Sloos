// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text;
using Sloos.Pump.EntityFramework;

namespace Sloos.Pump.TextTemplate
{
    public sealed class ContextFactory : IFactory
    {
        private readonly ContextView contextView;
        private readonly EntityNamespace nameSpace;

        public ContextFactory(ContextView contextView, EntityNamespace entityNamespace)
        {
            this.contextView = contextView;
            this.nameSpace = entityNamespace;
        }

        public string Name => this.contextView.Name;
        public string ModelNamespace => this.nameSpace.ModelNamespace;
        public string MappingNamespace => this.nameSpace.MappingNamespace;
        public ContextEntityView[] ContextEntityViews => this.contextView.ContextEntityViews;

        public void WriteTo(StringBuilder sb)
        {
            var template = new Context
            {
                Factory = this,
            };

            sb.AppendLine(
                template.TransformText());
        }
    }
}
