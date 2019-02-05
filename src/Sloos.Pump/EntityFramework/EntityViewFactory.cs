// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class EntityViewFactory
    {
        private readonly CodeGenEscaper codeGenEscaper;
        private readonly EntityType entityType;

        public EntityViewFactory(CodeGenEscaper codeGenEscaper, EntityType entityType)
        {
            this.codeGenEscaper = codeGenEscaper;
            this.entityType = entityType;

            this.SetEntityConstructorViews();
            this.SetEntityEntityViews();
            this.SetEntityNavigationViews();
        }

        public EntityConstructorView[] EntityConstructorViews { get; private set; }
        public EntityEntityView[] EntityEntityViews { get; private set; }
        public EntityNavigationView[] EntityNavigationViews { get; private set; }

        private void SetEntityConstructorViews()
        {
            var factory = new EntityConstructorViewFactory(this.codeGenEscaper);
            this.EntityConstructorViews = factory.Create(this.entityType).ToArray();
        }

        private void SetEntityEntityViews()
        {
            var factory = new EntityEntityViewFactory(this.codeGenEscaper);
            this.EntityEntityViews = factory.Create(this.entityType).ToArray();
        }

        private void SetEntityNavigationViews()
        {
            var factory = new EntityNavigationViewFactory(this.codeGenEscaper);
            this.EntityNavigationViews = factory.Create(this.entityType).ToArray();
        }
    }
}
