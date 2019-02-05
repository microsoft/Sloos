// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Data.Entity.Core.Metadata.Edm;
using FluentAssertions;
using Xunit;

namespace Sloos.Pump.Test.Builder
{
    public class EmdPropertyBuilderTest
    {
        [Fact]
        public void Int32ShouldBeConstructed()
        {
            var builder = new EdmPropertyBuilder();
            var property = builder.Build<int>("ID");

            property.Should().NotBeNull();
            property.Name.Should().Be("ID");
            property.PrimitiveType.Should().Be(PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32));
        }

        [Fact]
        public void StringShouldBeConstructed()
        {
            var builder = new EdmPropertyBuilder();
            var property = builder.Build<string>("Name");

            property.Should().NotBeNull();
            property.Name.Should().Be("Name");
            property.PrimitiveType.Should().Be(PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String));
        }
    }

    public class EdmPropertyBuilder
    {
        private readonly TypeUsageFactory typeUsageFactory;

        public EdmPropertyBuilder()
        {
            this.typeUsageFactory = new TypeUsageFactory();
        }

        public EdmProperty Build<T>(string name)
        {
            return this.Build<T>(name, x => { });
        }

        public EdmProperty Build<T>(string name, Action<EdmProperty> action)
        {
            var property = EdmProperty.Create(name, this.typeUsageFactory.Create<T>());
            action(property);

            return property;
        }

        public EdmProperty BuildString(string name, bool isUnicode, bool isFixedLength)
        {
            var typeUsage = TypeUsage.CreateStringTypeUsage(
                PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String),
                isUnicode,
                isFixedLength);

            var property = EdmProperty.Create(
                name,
                typeUsage);

            return property;
        }

        public EdmProperty BuildString(string name, bool isUnicode, bool isFixedLength, int maxLength)
        {
            var typeUsage = TypeUsage.CreateStringTypeUsage(
                PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String),
                isUnicode,
                isFixedLength,
                maxLength);

            var property = EdmProperty.Create(
                name,
                typeUsage);

            return property;
        }

        public EdmProperty BuildBinary(string name, bool isFixedLength)
        {
            var typeUsage = TypeUsage.CreateBinaryTypeUsage(
                PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Binary),
                isFixedLength);

            var property = EdmProperty.Create(
                name,
                typeUsage);

            return property;
        }

        public EdmProperty BuildBinary(string name, bool isFixedLength, int maxLength)
        {
            var typeUsage = TypeUsage.CreateBinaryTypeUsage(
                PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Binary),
                isFixedLength,
                maxLength);

            var property = EdmProperty.Create(
                name,
                typeUsage);

            return property;
        }
    }
}
