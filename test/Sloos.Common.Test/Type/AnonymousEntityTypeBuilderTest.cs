// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using System.Runtime.Serialization;
using FluentAssertions.Common;
using Xunit;

namespace Sloos.Common.Test
{
    public sealed class AnonymousEntityTypeBuilderTest
    {
        [Fact]
        public void BasicAnonymousType()
        {
            var testSubject = new AnonymousEntityTypeBuilder();

            var columns = new[]
            {
                new AnonymousEntityTypeProperty { Name = "Name", Type = typeof(string) },
                new AnonymousEntityTypeProperty { Name = "Value", Type = typeof(string) },
            };

            var type = testSubject.Create("UnitTest00", "Test00", columns);
            var instance = Activator.CreateInstance(type);

            Assert.True(instance.GetType().IsDecoratedWith<DataContractAttribute>());

            dynamic d = instance;

            Assert.Equal(0, (long)d.ID);
            Assert.Null((string)d.Name);
            Assert.Null((string)d.Value);

            d.Name = "--name--";
            d.Value = "--value--";

            Assert.Equal("--name--", (string)d.Name);
            Assert.Equal("--value--", (string)d.Value);

            Assert.Equal(3, type.GetProperties().Length);

            var idProperty = type.GetProperty("ID");
            Assert.False(idProperty.IsDecoratedWith<DataMemberAttribute>());

            var nameProperty = type.GetProperty("Name");
            Assert.True(nameProperty.IsDecoratedWith<DataMemberAttribute>());
            var nameAttr = nameProperty.CustomAttributes.First(x => x.AttributeType == typeof(DataMemberAttribute));
            Assert.Equal(0, (int)nameAttr.NamedArguments.First(x => x.MemberName == "Order").TypedValue.Value);

            var valueProperty = type.GetProperty("Value");
            Assert.True(valueProperty.IsDecoratedWith<DataMemberAttribute>());
            var valueAttr = valueProperty.CustomAttributes.First(x => x.AttributeType == typeof(DataMemberAttribute));
            Assert.Equal(1, (int)valueAttr.NamedArguments.First(x => x.MemberName == "Order").TypedValue.Value);
        }
    }
}
