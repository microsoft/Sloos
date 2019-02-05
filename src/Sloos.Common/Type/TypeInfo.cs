// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel;

namespace Sloos.Common
{
    public sealed class TypeInfo : IEquatable<TypeInfo>
    {
        private TypeInfo(TypeConverter converter, int order, Type type)
        {
            this.Converter = converter;
            this.Order = order;
            this.Type = type;
        }

        public TypeConverter Converter { get; }
        public int Order { get; }
        public Type Type { get; }

        public bool Equals(TypeInfo other)
        {
            return
                other != null &&
                this.Order == other.Order &&
                this.Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeInfo rhs && this.Equals(rhs);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Order.GetHashCode();
                hash = hash * 23 + this.Type.GetHashCode();
                return hash;
            }
        }

        public static TypeInfo Create(int order, Type type)
        {
            var typeInfo = new TypeInfo(
                TypeDescriptor.GetConverter(type),
                order,
                type);

            return typeInfo;
        }
    }
}
