// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;

using FluentAssertions;
using Xunit;

using Sloos.Common.Parse;

namespace Sloos.Common.Test
{
    public sealed class CsvSerializerFactoryTest
    {
        [Fact]
        public void SerializerFromRawCsv()
        {
            var testSubject = new CsvSerializerFactory();

            var csvSerializerState = testSubject.Create(nameof(this.SerializerFromRawCsv), nameof(this.SerializerFromRawCsv), TestData.SacramentoBee_RealEstate.ToStream());

            var serializerType = csvSerializerState.Serializer.GetType();
            serializerType.IsClass.Should().BeTrue();
            serializerType.IsConstructedGenericType.Should().BeTrue();
            serializerType.GenericTypeArguments.Should().HaveCount(1);

            //street,city,zip,state,beds,baths,sq__ft,type,sale_date,price,latitude,longitude
            //3526 HIGH ST, SACRAMENTO,95838,CA,2,1,836,Residential,Wed May 21 00:00:00 EDT 2008,59222,38.631913,-121.434879
            var entityType = serializerType.GenericTypeArguments[0];
            var entityTypeProperties = entityType.GetProperties();
            entityTypeProperties.Should().HaveCount(1 /*ID*/ + 12);

            entityTypeProperties.Select(x => x.Name).Should().ContainInOrder(
                "ID",
                "street",
                "city",
                "zip",
                "state",
                "beds",
                "baths",
                "sq__ft",
                "type",
                "sale_date",
                "price",
                "latitude",
                "longitude");

            // NOTE: zip really is an int, but it should probably be a string
            entityTypeProperties.Select(x => x.PropertyType).Should().ContainInOrder(
                typeof(int),     // ID
                typeof(string),  // street
                typeof(string),  // city
                typeof(int),     // zip
                typeof(string),  // state
                typeof(int),     // beds
                typeof(int),     // baths
                typeof(int),     // sq__ft
                typeof(string),  // type
                typeof(string),  // sale_date
                typeof(int),     // price
                typeof(double),  // latitude
                typeof(double)); // longitude
        }
    }
}
