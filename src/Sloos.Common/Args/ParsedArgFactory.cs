// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sloos
{
    public sealed class ParsedArgFactory
    {
        private readonly PropertyInfo[] propertyInfos;
        private readonly Type type;

        public ParsedArgFactory(Type type)
        {
            this.type = type;
            this.propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            this.UnparsedArgs = this.propertyInfos.SingleOrDefault(this.HasUnparsedArgumentsAttribute);

            this.ValidateUnparsedArgsType();
        }

        public PropertyInfo UnparsedArgs { get; }

        public Dictionary<string, ParsedArg> Create()
        {
            var parsedArgs = this.propertyInfos
                .Where(this.HasOptionAttribute)
                .Select(x => ParsedArg.Create(this.type, x))
                .ToArray();

            var longNameDictionary = parsedArgs
                .ToDictionary(x => x.LongName, x => x);

            var shortNameDictionary = parsedArgs
                .Where(x => x.HasShortName)
                .ToDictionary(x => x.ShortName, x => x);

            return longNameDictionary.Concat(shortNameDictionary).ToDictionary(x => x.Key, x => x.Value);
        }

        private bool HasOptionAttribute(PropertyInfo propertyInfo)
        {
            return this.GetOptionAttribute(propertyInfo) != null;
        }

        private OptionAttribute GetOptionAttribute(MemberInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<OptionAttribute>();
        }

        private bool HasUnparsedArgumentsAttribute(PropertyInfo propertyInfo)
        {
            return this.GetUnparsedArgumentsAttribute(propertyInfo) != null;
        }

        private UnparsedArgumentsAttribute GetUnparsedArgumentsAttribute(MemberInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<UnparsedArgumentsAttribute>();
        }

        private void ValidateUnparsedArgsType()
        {
            if (this.UnparsedArgs == null)
            {
                return;
            }

            if (!this.IsSupportedUnparsedArgsType(this.UnparsedArgs))
            {
                string message = $"The UnparsedArguments attribute attached to property named {this.UnparsedArgs.Name} must be of type string[] or IEnumerable<string>.";
                throw new ArgumentException(message);
            }
        }

        private bool IsSupportedUnparsedArgsType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(string[]) || 
                propertyInfo.PropertyType.GetInterfaces().Any(x => x == typeof(IEnumerable<string>));
        }

        public static Dictionary<string, ParsedArg> Create<T>()
            where T : new()
        {
            var factory = new ParsedArgFactory(typeof(T));
            return factory.Create();
        }
    }
}
