// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingPropertyViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;

        public MappingPropertyViewFactory(CodeGenEscaper codeGenEscaper)
        {
            this.codeGenEscaper = codeGenEscaper;
        }

        public IEnumerable<MappingPropertyView> Create(EntityType entityType)
        {
            return entityType.Properties
                .Select(x => new MappingProperty(x))
                .Select(x => this.Create(entityType, x));
        }

        private MappingPropertyView Create(EntityType entityType, MappingProperty mappingProperty)
        {
            var mappingPropertyView = new MappingPropertyView();

            mappingPropertyView.ConceptualName = mappingProperty.Name;
            mappingPropertyView.Attributes.AddRange(
                this.CreateAttributes(entityType, mappingProperty));

            return mappingPropertyView;
        }

        private IEnumerable<string> CreateAttributes(EntityType entityType, MappingProperty mappingProperty)
        {
            var mappingViewIsRequired = new MappingPropertyMatcherIsRequired();
            if (mappingViewIsRequired.IsMatch(mappingProperty))
            {
                yield return ".IsRequired()";
            }

            var mappingViewIsFixedLength = new MappingPropertyMatcherIsFixedLength();
            if (mappingViewIsFixedLength.IsMatch(mappingProperty))
            {
                yield return ".IsFixedLength()";
            }

            var mappingViewMaxLength = new MappingPropertyMatcherMaxLength();
            if (mappingViewMaxLength.IsMatch(mappingProperty))
            {
                yield return $".HasMaxLength({mappingViewMaxLength.GetMaxLength(mappingProperty)})";
            }

            var mappingViewIsRowVersion = new MappingPropertyMatcherIsRowVersion();
            if (mappingViewIsRowVersion.IsMatch(mappingProperty))
            {
                yield return ".IsRowVersion()";
            }

            var mappingViewHasStoreGeneratedPattern = new MappingPropertyMatcherHasStoreGeneratedPattern(entityType);
            if (mappingViewHasStoreGeneratedPattern.IsMatch(mappingProperty))
            {
                yield return $".HasDatabaseGeneratedOption(DatabaseGeneratedOption.{mappingViewHasStoreGeneratedPattern.GetStoreGeneratedPattern(mappingProperty)})";
            }
        }
    }
}
