// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Sloos.Common
{
    public class TypeInfo : IEquatable<TypeInfo>
    {
        protected TypeInfo(TypeConverter converter, int order, Type type)
        {
            this.Converter = converter;
            this.Order = order;
            this.Type = type;
        }

        public TypeConverter Converter { get; }
        public int Order { get; }
        public Type Type { get; }

        public virtual bool Maybe(string s) => true;

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

    public sealed class GuidTypeInfo : TypeInfo
    {
        private GuidTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(Guid)), order, typeof(Guid))
        {
        }

        public override bool Maybe(string s)
        {
            return Guid.TryParse(s, out Guid _);
        }

        public static GuidTypeInfo Create(int order)
        {
            return new GuidTypeInfo(order);
        }
    }

    public sealed class IntTypeInfo : TypeInfo
    {
        private IntTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(int)), order, typeof(int))
        {
        }

        public override bool Maybe(string s)
        {
            return int.TryParse(s, out int _);
        }

        public static IntTypeInfo Create(int order)
        {
            return new IntTypeInfo(order);
        }
    }

    public sealed class LongTypeInfo : TypeInfo
    {
        private LongTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(long)), order, typeof(long))
        {
        }

        public override bool Maybe(string s)
        {
            return long.TryParse(s, out long _);
        }

        public static LongTypeInfo Create(int order)
        {
            return new LongTypeInfo(order);
        }
    }

    public sealed class ULongTypeInfo : TypeInfo
    {
        private ULongTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(ulong)), order, typeof(ulong))
        {
        }

        public override bool Maybe(string s)
        {
            return ulong.TryParse(s, out ulong _);
        }

        public static ULongTypeInfo Create(int order)
        {
            return new ULongTypeInfo(order);
        }
    }

    public sealed class DoubleTypeInfo : TypeInfo
    {
        private DoubleTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(double)), order, typeof(double))
        {
        }

        public override bool Maybe(string s)
        {
            return double.TryParse(s, out double _);
        }

        public static DoubleTypeInfo Create(int order)
        {
            return new DoubleTypeInfo(order);
        }
    }

    public sealed class CharTypeInfo : TypeInfo
    {
        private CharTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(char)), order, typeof(char))
        {
        }

        public override bool Maybe(string s)
        {
            return char.TryParse(s, out char _);
        }

        public static CharTypeInfo Create(int order)
        {
            return new CharTypeInfo(order);
        }
    }

    public sealed class BoolTypeInfo : TypeInfo
    {
        private BoolTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(bool)), order, typeof(bool))
        {
        }

        public override bool Maybe(string s)
        {
            return bool.TryParse(s, out bool _);
        }

        public static BoolTypeInfo Create(int order)
        {
            return new BoolTypeInfo(order);
        }
    }

    public sealed class TimeSpanTypeInfo : TypeInfo
    {
        private TimeSpanTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(TimeSpan)), order, typeof(TimeSpan))
        {
        }

        public override bool Maybe(string s)
        {
            return TimeSpan.TryParse(s, out TimeSpan _);
        }

        public static TimeSpanTypeInfo Create(int order)
        {
            return new TimeSpanTypeInfo(order);
        }
    }

    public sealed class DateTimeTypeInfo : TypeInfo
    {
        private DateTimeTypeInfo(int order)
            : base(TypeDescriptor.GetConverter(typeof(DateTime)), order, typeof(DateTime))
        {
        }

        public override bool Maybe(string s)
        {
            return DateTime.TryParse(s, out DateTime _);
        }

        public static DateTimeTypeInfo Create(int order)
        {
            return new DateTimeTypeInfo(order);
        }
    }
}
