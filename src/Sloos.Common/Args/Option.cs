// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Sloos
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionAttribute : Attribute
    {
        public OptionAttribute()
        {
            this.Delimiter = ",";
        }

        public string Delimiter { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public string Pattern { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; } 
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UnparsedArgumentsAttribute : Attribute
    {
    }
}
