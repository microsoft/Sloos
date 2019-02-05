// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using System.Reflection;

namespace Sloos
{
    public class ParsedArg
    {
        private readonly PropertyInfo isRequiredPropertyInfo;
        private readonly OptionAttribute option;
        private readonly PropertyInfo propertyInfo;

        private ParsedArg(PropertyInfo propertyInfo, PropertyInfo isRequiredPropertyInfo, OptionAttribute option)
        {
            this.propertyInfo = propertyInfo;
            this.isRequiredPropertyInfo = isRequiredPropertyInfo;

            this.option = option;
        }

        public PropertyInfo PropertyInfo => this.propertyInfo;

        public string DefaultValue => this.option.DefaultValue;
        public string LongName => string.IsNullOrWhiteSpace(this.option.LongName) ? ArgsHelpers.PropertyToOptionName(this.propertyInfo.Name) : this.option.LongName;
        public string ShortName => this.option.ShortName;

        public bool HasShortName => !string.IsNullOrWhiteSpace(this.ShortName);
        public bool HasValue => this.propertyInfo.PropertyType != typeof(bool);
        public bool IsRequired => this.GetIsRequired();

        private bool GetIsRequired()
        {
            Type type = this.propertyInfo.PropertyType;

            if (this.HasDefaultValue())
            {
                return false;
            }

            if (type.IsValueType)
            {
                return Nullable.GetUnderlyingType(type) == null;
            }

            return this.isRequiredPropertyInfo == null;
        }

        private bool HasDefaultValue()
        {
            return !string.IsNullOrWhiteSpace(this.DefaultValue);
        }

        public static ParsedArg Create(Type type, PropertyInfo propertyInfo)
        {
            OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>();
            if (optionAttribute == null)
            {
                string message = $"The property {propertyInfo.Name} does not have an [Option] attribute.";
                throw new ArgumentException(message);
            }

            string isRequiredPropertyName = $"Is{propertyInfo.Name}";
            var isRequiredPropertyInfo = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SingleOrDefault(x => x.Name == isRequiredPropertyName);

            return new ParsedArg(propertyInfo, isRequiredPropertyInfo, optionAttribute);
        }
    }
}
