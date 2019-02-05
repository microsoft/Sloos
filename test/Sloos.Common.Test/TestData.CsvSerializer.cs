// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.Serialization;

namespace Sloos.Common.Test
{
    public static partial class TestData
    {
        public class ClassWithoutDataContractAttribute
        {
            public int ID { get; set; }
            [DataMember] public string Field { get; set; }
        }

        [DataContract]
        public class ComplexOrderByThenBy
        {
            [DataMember(Order = 0)] public string Aad { get; set; }
            [DataMember(Order = 0)] public string Aac { get; set; }
            [DataMember(Order = 0)] public string aac { get; set; }

            [DataMember(Order = 5)] public string Aaa { get; set; }
            [DataMember(Order = 3)] public string Aab { get; set; }
        }

        [DataContract]
        public class ExplicitlyOrdered
        {
            [DataMember] public string Field0 { get; set; }
            [DataMember(Order = 3)] public string Field3 { get; set; }
            [DataMember(Order = 1)] public string Field1 { get; set; }
            [DataMember(Order = 2)] public string Field2 { get; set; }
        }

        [DataContract]
        public class ImplicitlyOrdered
        {
            [DataMember] public string Field1 { get; set; }
            [DataMember] public string Field2 { get; set; }
            [DataMember] public string Field3 { get; set; }
        }

        [DataContract]
        public class MixedNullablePropertyTypes
        {
            [DataMember] public byte? Field01 { get; set;}
            [DataMember] public sbyte? Field02 { get; set;}
            [DataMember] public char? Field03 { get; set;}
            [DataMember] public short? Field04 { get; set;}
            [DataMember] public ushort? Field05 { get; set;}
            [DataMember] public int? Field06 { get; set;}
            [DataMember] public uint? Field07 { get; set;}
            [DataMember] public long? Field08 { get; set;}
            [DataMember] public ulong? Field09 { get; set;}
            [DataMember] public float? Field10 { get; set;}
            [DataMember] public double? Field11 { get; set;}
            [DataMember] public decimal? Field12 { get; set;}
            [DataMember] public Guid? Field13 { get; set;}
            [DataMember] public TimeSpan? Field14 { get; set;}
            [DataMember] public DateTime? Field15 { get; set;}
            [DataMember] public DateTimeOffset? Field16 { get; set;}
        }

        [DataContract]
        public class MixedValuePropertyTypes
        {
            [DataMember] public byte Field01 { get; set;}
            [DataMember] public sbyte Field02 { get; set;}
            [DataMember] public char Field03 { get; set;}
            [DataMember] public short Field04 { get; set;}
            [DataMember] public ushort Field05 { get; set;}
            [DataMember] public int Field06 { get; set;}
            [DataMember] public uint Field07 { get; set;}
            [DataMember] public long Field08 { get; set;}
            [DataMember] public ulong Field09 { get; set;}
            [DataMember] public float Field10 { get; set;}
            [DataMember] public double Field11 { get; set;}
            [DataMember] public decimal Field12 { get; set;}
            [DataMember] public Guid Field13 { get; set;}
            [DataMember] public TimeSpan Field14 { get; set;}
            [DataMember] public DateTime Field15 { get; set;}
            [DataMember] public DateTimeOffset Field16 { get; set;}
        }

        [DataContract]
        public class NamedMembers
        {
            [DataMember(Name = "Field")]
            public string Field { get; set; }
        }

        [DataContract]
        public class OrderByOrderThenByName
        {
            [DataMember] public string Field3 { get; set; }
            [DataMember] public string Field2 { get; set; }
            [DataMember] public string Field1 { get; set; }
        }

        [DataContract]
        public class Simple
        {
            public int ID { get; set; }
            [DataMember] public string Field { get; set; }
        }
    }
}
