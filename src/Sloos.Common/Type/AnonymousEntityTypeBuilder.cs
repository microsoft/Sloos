// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;   
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Sloos.Common
{
    public sealed class AnonymousEntityTypeProperty
    {
        public Type Type { get; set; }
        public string Name { get; set; }
    }

    // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.assemblybuilder
    public sealed class AnonymousEntityTypeBuilder
    {
        public Type Create(string assemblyName, string typeName, AnonymousEntityTypeProperty[] columns)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            var typeBuilder = assemblyBuilder
                .DefineDynamicModule(assemblyName)
                .DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);

            this.ApplyCustomAttribute<DataContractAttribute>(
                x => typeBuilder.SetCustomAttribute(x));

            // NOTE: this should not be hard coded!
            this.CreateProperty(typeBuilder, typeof(int), "ID");

            for (int i=0; i < columns.Length; i++)
            {
                var propertyBuilder = this.CreateProperty(typeBuilder, columns[i].Type, columns[i].Name);
                this.AddDataMemberAttribute(propertyBuilder, i);
            }

            return typeBuilder.CreateType();
        }

        private PropertyBuilder CreateProperty(TypeBuilder typeBuilder, Type type, string name)
        {
            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, new[] { type });

            var fieldBuilder = typeBuilder.DefineField($"_{name}", type, FieldAttributes.Private);

            propertyBuilder.SetGetMethod(
                this.CreateGetMethod(typeBuilder, fieldBuilder, type, name));
            propertyBuilder.SetSetMethod(
                this.CreateSetMethod(typeBuilder, fieldBuilder, type, name));

            return propertyBuilder;
        }

        private void ApplyCustomAttribute<T>(Action<CustomAttributeBuilder> action)
        {
            var dataMemberAttr = typeof(T);
            var attrBuilder = new CustomAttributeBuilder(
                dataMemberAttr.GetConstructor(Type.EmptyTypes),
                new object[] { });

            action(attrBuilder);
        }

        private void AddDataMemberAttribute(PropertyBuilder propertyBuilder, int order)
        {
            var dataMemberAttr = typeof(DataMemberAttribute);
            var propertyInfos = new[] { dataMemberAttr.GetProperty("Order") };
            var propertyValues = new object[] { order };

            var attrBuilder = new CustomAttributeBuilder(
                dataMemberAttr.GetConstructor(Type.EmptyTypes),
                new object[] { },
                propertyInfos,
                propertyValues);

            propertyBuilder.SetCustomAttribute(attrBuilder);
        }

        private MethodBuilder CreateGetMethod(TypeBuilder typeBuilder, FieldBuilder field, Type type, string name)
        {
            var methodBuilder = typeBuilder.DefineMethod(
                $"get_{name}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                type,
                Type.EmptyTypes);

            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            il.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        private MethodBuilder CreateSetMethod(TypeBuilder typeBuilder, FieldBuilder field, System.Type type, string name)
        {
            var methodBuilder = typeBuilder.DefineMethod(
                $"set_{name}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new[] { type });

            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);

            return methodBuilder;
        }
    }
}
