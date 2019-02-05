// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.EntityFramework
{
    public sealed class MappingProperty
    {
        private readonly EdmProperty edmProperty;

        public MappingProperty(EdmProperty edmProperty)
        {
            this.edmProperty = edmProperty;
        }

        public string Name => this.edmProperty.Name;
        public string TypeName => this.edmProperty.TypeName;

        public bool IsNullable => this.edmProperty.Nullable;

        public bool IsStoreGeneratedIdentity => this.edmProperty.IsStoreGeneratedIdentity;
        public bool IsStoreGeneratedComputed => this.edmProperty.IsStoreGeneratedComputed;

        public bool IsTypeOfString()    { return this.GetClrEquivalentType() == typeof(string); }
        public bool IsTypeOfByteArray() { return this.GetClrEquivalentType() == typeof(byte[]); }

        public bool HasFacet(string facetName) { return this.edmProperty.TypeUsage.Facets.Any(x => x.Name == facetName); }
        
        public Facet GetFacet(string facetName)
        {
            return this.edmProperty.TypeUsage.Facets.Single(x => x.Name == facetName);
        }

        public T GetFacetValue<T>(string facetName)
        {
            return (T)this.GetFacet(facetName).Value;
        }

        public Type GetClrEquivalentType()
        {
            return ((PrimitiveType)this.edmProperty.TypeUsage.EdmType).ClrEquivalentType;
        }

        public bool IsTypeOfNumericKey()
        {
            return
                this.GetClrEquivalentType() == typeof(short) ||
                this.GetClrEquivalentType() == typeof(int) ||
                this.GetClrEquivalentType() == typeof(long) ||
                this.GetClrEquivalentType() == typeof(decimal);
        }
    }
}
