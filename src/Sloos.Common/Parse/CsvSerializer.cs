// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sloos.Common
{
    // XXX: C++ templates would be good right about now...

    public class CsvSerializer<T>
        where T : class, new()
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Dictionary<Type, Func<string, object>> CoercionMap;

        private readonly PropertyInfo[] propertyInfos;
        private readonly Action<T, object>[] propertySetters;
        private readonly Type type;

        static CsvSerializer()
        {
            CsvSerializer<T>.CoercionMap = CoercionHelpers.CreateCoercionMap();
        }

        public CsvSerializer()
        {
            this.type = typeof(T);
            this.propertyInfos = CsvSerializerHelpers.OrderByProperty(this.type)
                .ToArray();

            this.propertySetters = this.propertyInfos.Select(CoercionHelpers.CreatePropertySetter<T>)
                .ToArray();

            if (!this.IsDataContract())
            {
                string message = $@"The type ""{this.type.FullName}"" does not have the [DataContract] attribute!";
                throw new ArgumentException(message);
            }
        }

        public IEnumerable<T> Deserialize(Stream stream)
        {
            return this.Deserialize(stream, ',');
        }

        public IEnumerable<T> Deserialize(Stream stream, char delimiter, int skipLineCount=0)
        {
            var settings = this.GetDelimitedParserSettings(delimiter);

            var parser = DelimitedParser.Create(
                settings,
                stream);

            return parser.Parse()
                .Skip(skipLineCount)
                .Select(row => this.MapArrayOntoType(row.ToArray()));
        }

        private DelimitedParserSettings GetDelimitedParserSettings(char delimiter)
        {
            var factory = new CsvSerializerHeaderFactory(this.type);

            var settings = new DelimitedParserSettings
            {
                DelimitedHeader = new DelimitedHeader(factory.GetDelimitedColumns()), 
                Delimiter = delimiter,
            };

            return settings;
        }

        private T MapArrayOntoType(string[] values)
        {
            var instance = (T)Activator.CreateInstance(this.type);

            for (int i=0; i < values.Length; i++)
            {
                var setter = this.propertySetters[i];
                var value = CsvSerializer<T>.CoercionMap[this.propertyInfos[i].PropertyType].Invoke(values[i]);
                setter(instance, value);
            }

            return instance;
        }

        private bool IsDataContract()
        {
            return this.type.CustomAttributes.Any(x => x.AttributeType == typeof(DataContractAttribute));
        }
    }
}
