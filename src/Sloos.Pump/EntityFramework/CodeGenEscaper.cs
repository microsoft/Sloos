// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Globalization;
using Microsoft.CSharp;

namespace Sloos.Pump.EntityFramework
{
    public sealed class CodeGenEscaper
    {
        private readonly CSharpCodeProvider provider;

        public CodeGenEscaper()
        {
            this.provider = new CSharpCodeProvider();
        }

        public string Escape(EdmMember edmMember)
        {
            return this.Escape(edmMember.Name);
        }

        public string Escape(TypeUsage typeUsage)
        {
            if (typeUsage.EdmType is ComplexType || typeUsage.EdmType is EntityType)
            {
                return this.Escape(typeUsage.EdmType.Name);
            }
            
            if (typeUsage.EdmType is SimpleType)
            {
                Type clrType = this.UnderlyingClrType(typeUsage.EdmType);
                string typeName = typeUsage.EdmType is EnumType ? this.Escape(typeUsage.EdmType.Name) : this.Escape(clrType);
                if (clrType.IsValueType && this.IsNullable(typeUsage))
                {
                    return string.Format(CultureInfo.InvariantCulture, "global::System.Nullable<{0}>", typeName);
                }

                return typeName;
            }

            if (typeUsage.EdmType is CollectionType collectionType)
            {
                return this.EscapeCollection(this.Escape(collectionType.TypeUsage));
            }

            throw new ArgumentException("typeUsage");
        }

        /// <summary>
        /// Returns the name of the Type object formatted for
        /// use in source code.
        /// </summary>
        public string Escape(Type clrType)
        {
            string typeName = "global::" + clrType.FullName;
            return typeName;
        }

        public string Escape(string name)
        {
            return this.provider.CreateEscapedIdentifier(name);
        }

        /// <summary>
        /// This method returns the underlying CLR type given the c-space type.
        /// Note that for an enum type this means that the type backing the enum will be returned, not the enum type itself.
        /// </summary>
        public Type UnderlyingClrType(EdmType edmType)
        {
            switch (edmType)
            {
                case PrimitiveType primitiveType:
                    return primitiveType.ClrEquivalentType;
                case EnumType enumType:
                    return enumType.UnderlyingType.ClrEquivalentType;
                default:
                    return typeof(object);
            }
        }

        /// <summary>
        /// True if the EdmProperty TypeUsage is Nullable, False otherwise.
        /// </summary>
        public bool IsNullable(EdmProperty property)
        {
            return property != null && this.IsNullable(property.TypeUsage);
        }

        /// <summary>
        /// True if the TypeUsage is Nullable, False otherwise.
        /// </summary>
        public bool IsNullable(TypeUsage typeUsage)
        {
            if (typeUsage != null &&
                typeUsage.Facets.TryGetValue("Nullable", true, out Facet nullableFacet))
            {
                return (bool)nullableFacet.Value;
            }

            return false;
        }

        public string EscapeCollection(string typeName)
        {
            return string.Format(CultureInfo.InvariantCulture, "global::System.Collections.Generic.ICollection<{0}>", typeName);
        }

        public string EscapeList(string typeName)
        {
            return string.Format(CultureInfo.InvariantCulture, "global::System.Collections.Generic.List<{0}>", typeName);
        }
    }
}
