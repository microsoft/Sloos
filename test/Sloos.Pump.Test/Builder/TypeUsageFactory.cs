// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace Sloos.Pump.Test.Builder
{
    public class TypeUsageFactory
    {
        private static readonly Dictionary<Type, PrimitiveTypeKind> TypeMap;

        static TypeUsageFactory()
        {
            // XXX: not complete by any means, e.g. complex, enum, etc.
            TypeUsageFactory.TypeMap = new Dictionary<Type, PrimitiveTypeKind>()
                                           {
                                               { typeof(System.Decimal), PrimitiveTypeKind.Decimal },
                                               { typeof(System.Int16), PrimitiveTypeKind.Int16 },
                                               { typeof(System.Int32), PrimitiveTypeKind.Int32 },
                                               { typeof(System.Int64), PrimitiveTypeKind.Int64 },
                                               { typeof(System.String), PrimitiveTypeKind.String },
                                           };
        }

        public TypeUsage Create<T>()
        {
            return this.Create(typeof(T));
        }

        public TypeUsage Create(Type type)
        {
            var typeUsage = TypeUsage.Create(
                this.GetPrimitiveType(type),
                Enumerable.Empty<Facet>());

            return typeUsage;
        }

        private PrimitiveType GetPrimitiveType(Type type)
        {
            if (!TypeUsageFactory.TypeMap.ContainsKey(type))
            {
                string message = $"The type '{type.FullName}' is not implemented!";
                throw new NotImplementedException(message);
            }

            return PrimitiveType.GetEdmPrimitiveType(
                TypeUsageFactory.TypeMap[type]);
        }
    }
}
