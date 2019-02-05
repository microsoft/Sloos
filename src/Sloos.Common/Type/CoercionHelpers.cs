// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Sloos
{
    public static class CoercionHelpers
    {
        public static Action<TType, object> CreatePropertySetter<TType>(PropertyInfo propertyInfo)
        {
            // Expression of the form:
            //   instance.Property = (PropertyType)value;
            var target = Expression.Parameter(typeof(TType), "obj");
            var value = Expression.Parameter(typeof(object), "value");

            var body = Expression.Assign(
                Expression.Property(target, propertyInfo),
                Expression.Convert(value, propertyInfo.PropertyType));

            // XXX: Type coercion is _currently_ a distinct activity.  I tend to
            // think that this is better because I can easily override the 
            // behavior as appropriate.  The coolness factor is lost because 
            // who doesn't like writing code at run time?
            //
            // var s = Expression.Parameter(typeof(string));
            // var asInt = Expression.Call(typeof(int), "Parse", null, s);
            // var lambda = Expression.Lambda<Func<string, int>>(asInt, s);

            var lambda = Expression.Lambda<Action<TType, object>>(body, target, value);
            return lambda.Compile();
        }

        public static Dictionary<Type, Func<string, object>> CreateCoercionMap()
        {
            // XXX: I could move the Trim() outside of this method, but I don't
            // want that logic sitting "outside" of the thing in the know.
            return new Dictionary<Type, Func<string, object>>()
            {
                // value types
                { typeof(byte), x => byte.Parse(x.Trim()) },
                { typeof(bool), x => bool.Parse(x.Trim()) },
                { typeof(sbyte), x => sbyte.Parse(x.Trim()) },
                { typeof(char), x => char.Parse(x.Trim()) },
                { typeof(short), x => short.Parse(x.Trim()) },
                { typeof(ushort), x => ushort.Parse(x.Trim()) },
                { typeof(int), x => int.Parse(x.Trim()) },
                { typeof(uint), x => uint.Parse(x.Trim()) },
                { typeof(long), x => long.Parse(x.Trim()) },
                { typeof(ulong), x => ulong.Parse(x.Trim()) },
                { typeof(float), x => float.Parse(x.Trim()) },
                { typeof(double), x => double.Parse(x.Trim()) },
                { typeof(decimal), x => decimal.Parse(x.Trim()) },
                { typeof(Guid), x => Guid.Parse(x.Trim()) },
                { typeof(TimeSpan), x => TimeSpan.Parse(x.Trim()) },

                // http://msdn.microsoft.com/en-us/library/91hfhz89.aspx 
                // AdjustToUniversal :: Date and time are returned as a Coordinated Universal Time (UTC). If
                // the input string denotes a local time, through a time zone specifier or AssumeLocal, the 
                // date and time are converted from the local time to UTC. If the input string denotes a UTC 
                // time, through a time zone specifier or AssumeUniversal, no conversion occurs. If the input 
                // string does not denote a local or UTC time, no conversion occurs and the resulting Kind 
                // property is Unspecified.  This value cannot be used with RoundtripKind.
                { typeof(DateTime), x => DateTime.Parse(x.Trim(), System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AdjustToUniversal) },
                
                { typeof(DateTimeOffset), x => DateTimeOffset.Parse(x.Trim()) },
                // reference type
                { typeof(string), x => x },
                // nullable types
                { typeof(byte?), x => string.IsNullOrWhiteSpace(x) ? null : (byte?)byte.Parse(x.Trim()) },
                { typeof(bool?), x => string.IsNullOrWhiteSpace(x) ? null : (bool?)bool.Parse(x.Trim()) },
                { typeof(sbyte?), x => string.IsNullOrWhiteSpace(x) ? null : (sbyte?)sbyte.Parse(x.Trim()) },
                { typeof(char?), x => string.IsNullOrWhiteSpace(x) ? null : (char?)char.Parse(x.Trim()) },
                { typeof(short?), x => string.IsNullOrWhiteSpace(x) ? null : (short?)short.Parse(x.Trim()) },
                { typeof(ushort?), x => string.IsNullOrWhiteSpace(x) ? null : (ushort?)ushort.Parse(x.Trim()) },
                { typeof(int?), x => string.IsNullOrWhiteSpace(x) ? null : (int?)int.Parse(x.Trim()) },
                { typeof(uint?), x => string.IsNullOrWhiteSpace(x) ? null : (uint?)uint.Parse(x.Trim()) },
                { typeof(long?), x => string.IsNullOrWhiteSpace(x) ? null : (long?)long.Parse(x.Trim()) },
                { typeof(ulong?), x => string.IsNullOrWhiteSpace(x) ? null : (ulong?)ulong.Parse(x.Trim()) },
                { typeof(float?), x => string.IsNullOrWhiteSpace(x) ? null : (float?)float.Parse(x.Trim()) },
                { typeof(double?), x => string.IsNullOrWhiteSpace(x) ? null : (double?)double.Parse(x.Trim()) },
                { typeof(decimal?), x => string.IsNullOrWhiteSpace(x) ? null : (decimal?)decimal.Parse(x.Trim()) },
                { typeof(Guid?), x => string.IsNullOrWhiteSpace(x) ? null : (Guid?)Guid.Parse(x.Trim()) },
                { typeof(TimeSpan?), x => string.IsNullOrWhiteSpace(x) ? null : (TimeSpan?)TimeSpan.Parse(x.Trim()) },
                { typeof(DateTime?), x => string.IsNullOrWhiteSpace(x) ? null : (DateTime?)DateTime.Parse(x.Trim(), System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AdjustToUniversal) },
                { typeof(DateTimeOffset?), x => string.IsNullOrWhiteSpace(x) ? null : (DateTimeOffset?)DateTimeOffset.Parse(x.Trim()) },
            };
        }
    }
}
