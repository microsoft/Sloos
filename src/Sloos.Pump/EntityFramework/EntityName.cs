// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Sloos.Pump.EntityFramework
{
    public sealed class EntityName
    {
        private readonly string name;
        private readonly PluralizationService pluralizationService;

        public EntityName(string name)
        {
            this.name = name;
            this.pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public string StoreName => this.pluralizationService.Pluralize(this.name);
        public string ConceptualName => this.pluralizationService.Singularize(this.name);
        public string ConceptualMappingName => $"{this.ConceptualName}Map";
    }
}
