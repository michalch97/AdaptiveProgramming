using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveProgrammingModel
{
    [Serializable]
    public class TypeMetadata : ISerializable
    {
        private bool isSupplemented;
        private string typeName;
        private string namespaceName;
        private TypeMetadata baseType;
        private IEnumerable<TypeMetadata> genericArguments;
        private Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers;
        private TypeKind typeKind;
        private IEnumerable<Attribute> attributes;
        private IEnumerable<TypeMetadata> implementedInterfaces;
        private IEnumerable<TypeMetadata> nestedTypes;
        private IEnumerable<PropertyMetadata> properties;
        private TypeMetadata declaringType;
        private IEnumerable<MethodMetadata> methods;
        private IEnumerable<MethodMetadata> constructors;
        private TypeMetadata(string typeName, string namespaceName)
        {
            this.typeName = typeName;
            this.namespaceName = namespaceName;
            this.isSupplemented = false;
        }
        private TypeMetadata(string typeName, string namespaceName, IEnumerable<TypeMetadata> genericArguments) : this(typeName, namespaceName)
        {
            this.genericArguments = genericArguments;
        }
        
        public TypeMetadata(Type type)
        {
            this.typeName = type.Name;
            this.declaringType = EmitDeclaringType(type.DeclaringType);
            this.constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            this.methods = MethodMetadata.EmitMethods(type.GetMethods());
            this.nestedTypes = EmitNestedTypes(type.GetNestedTypes());
            this.implementedInterfaces = EmitImplements(type.GetInterfaces());
            this.genericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
            this.modifiers = EmitModifiers(type);
            this.baseType = EmitExtends(type.BaseType);
            this.properties = PropertyMetadata.EmitProperties(type.GetProperties());
            this.typeKind = GetTypeKind(type);
            this.attributes = type.GetCustomAttributes(false).Cast<Attribute>();
            this.isSupplemented = true;
        }
        public static TypeMetadata EmitReference(Type type)
        {
            long id = AssemblyLoader.idGenerator.GetId(type, out bool firstTime);
            if (firstTime)
            {
                if (!type.IsGenericType)
                    return new TypeMetadata(type.Name, type.GetNamespace());
                else
                    return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments()));
            }
            else
            {
                if (!type.IsGenericType)
                    return new TypeMetadata(type.Name, type.GetNamespace());
                else
                {
                    bool load = AssemblyLoader.loadedTypes.TryGetValue(id, out TypeMetadata newTypeMetadata);
                    if (newTypeMetadata.GetSupplemented())
                    {
                        return newTypeMetadata;
                    }
                    else
                    {
                        newTypeMetadata.typeName = type.Name;
                        newTypeMetadata.declaringType = EmitDeclaringType(type.DeclaringType);
                        newTypeMetadata.constructors = MethodMetadata.EmitMethods(type.GetConstructors());
                        newTypeMetadata.methods = MethodMetadata.EmitMethods(type.GetMethods());
                        newTypeMetadata.nestedTypes = EmitNestedTypes(type.GetNestedTypes());
                        newTypeMetadata.implementedInterfaces = EmitImplements(type.GetInterfaces());
                        newTypeMetadata.genericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
                        newTypeMetadata.modifiers = EmitModifiers(type);
                        newTypeMetadata.baseType = EmitExtends(type.BaseType);
                        newTypeMetadata.properties = PropertyMetadata.EmitProperties(type.GetProperties());
                        newTypeMetadata.typeKind = GetTypeKind(type);
                        newTypeMetadata.attributes = type.GetCustomAttributes(false).Cast<Attribute>();
                        newTypeMetadata.isSupplemented = true;
                        AssemblyLoader.loadedTypes[id] = newTypeMetadata;
                        return newTypeMetadata;
                    }
                }
            }
        }
        public static IEnumerable<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return from Type argument in arguments select EmitReference(argument);
        }
        public bool GetSupplemented()
        {
            return this.isSupplemented;
        }
        private static TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private static IEnumerable<TypeMetadata> EmitNestedTypes(IEnumerable<Type> nestedTypes)
        {
            return from type in nestedTypes
                   where type.GetVisible()
                   select new TypeMetadata(type);
        }
        private static IEnumerable<TypeMetadata> EmitImplements(IEnumerable<Type> interfaces)
        {
            return from currentInterface in interfaces
                   select EmitReference(currentInterface);
        }
        private static TypeKind GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKind.EnumType :
                type.IsValueType ? TypeKind.StructType :
                type.IsInterface ? TypeKind.InterfaceType :
                TypeKind.ClassType;
        }
        private static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }
        private static Tuple<AccessLevel, SealedEnum, AbstractEnum> EmitModifiers(Type type)
        {
            //set defaults 
            AccessLevel access = AccessLevel.IsPrivate;
            AbstractEnum abstractEnum = AbstractEnum.NotAbstract;
            SealedEnum sealedEnum = SealedEnum.NotSealed;
            // check if not default 
            if (type.IsPublic)
                access = AccessLevel.IsPublic;
            else if (type.IsNestedPublic)
                access = AccessLevel.IsPublic;
            else if (type.IsNestedFamily)
                access = AccessLevel.IsProtected;
            else if (type.IsNestedFamANDAssem)
                access = AccessLevel.IsProtectedInternal;
            if (type.IsSealed)
                sealedEnum = SealedEnum.Sealed;
            if (type.IsAbstract)
                abstractEnum = AbstractEnum.Abstract;
            return new Tuple<AccessLevel, SealedEnum, AbstractEnum>(access, sealedEnum, abstractEnum);
        }
        public string TypeName
        {
            get { return this.typeName; }
            private set { this.typeName = value; }
        }
        public IEnumerable<MethodMetadata> MethodsMetadata
        {
            get { return this.methods; }
            private set { this.methods = value; }
        }
        public IEnumerable<TypeMetadata> NestedTypesMetadata
        {
            get { return this.nestedTypes; }
            private set { this.nestedTypes = value; }
        }
        public IEnumerable<PropertyMetadata> Properties
        {
            get { return this.properties; }
            private set { this.properties = value; }
        }

        public TypeMetadata(SerializationInfo info, StreamingContext context)
        {
            isSupplemented = (bool)info.GetValue("isSupplemented",typeof(bool));
            typeName = (string)info.GetValue("typeName", typeof(string)); ;
            namespaceName = (string)info.GetValue("namespaceName", typeof(string));
            baseType = (TypeMetadata)info.GetValue("baseType", typeof(TypeMetadata));
            genericArguments = (IEnumerable<TypeMetadata>)info.GetValue("genericArguments", typeof(IEnumerable<TypeMetadata>));
            modifiers = (Tuple<AccessLevel, SealedEnum, AbstractEnum>)info.GetValue("modifiers", typeof(Tuple<AccessLevel, SealedEnum, AbstractEnum>));
            typeKind = (TypeKind)info.GetValue("typeKind", typeof(TypeKind));
            attributes = (IEnumerable<Attribute>)info.GetValue("attributes", typeof(IEnumerable<Attribute>));
            implementedInterfaces = (IEnumerable<TypeMetadata>)info.GetValue("implementedInterfaces", typeof(IEnumerable<TypeMetadata>));
            nestedTypes = (IEnumerable<TypeMetadata>)info.GetValue("nestedTypes", typeof(IEnumerable<TypeMetadata>));
            properties = (IEnumerable<PropertyMetadata>)info.GetValue("properties", typeof(IEnumerable<PropertyMetadata>));
            declaringType = (TypeMetadata)info.GetValue("declaringType", typeof(TypeMetadata));
            methods = (IEnumerable<MethodMetadata>)info.GetValue("methods", typeof(IEnumerable<MethodMetadata>));
            constructors = (IEnumerable<MethodMetadata>)info.GetValue("constructors", typeof(IEnumerable<MethodMetadata>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("isSupplemented",isSupplemented);
            info.AddValue("typeName", typeName);
            info.AddValue("namespaceName", namespaceName);
            info.AddValue("baseType", baseType);
            info.AddValue("genericArguments", genericArguments);
            info.AddValue("modifiers", modifiers);
            info.AddValue("typeKind", typeKind);
            info.AddValue("attributes", attributes);
            info.AddValue("implementedInterfaces", implementedInterfaces);
            info.AddValue("nestedTypes", nestedTypes);
            info.AddValue("properties", properties);
            info.AddValue("declaringType", declaringType);
            info.AddValue("methods", methods);
            info.AddValue("constructors", constructors);
        }
    }
}
