using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingModel
{
    public class TypeMetadata : TypeBase
    {
        public bool IsSupplemented { get; set; }
        public override string TypeName { get; set; }
        public override string NamespaceName { get; set; }
        public override TypeBase BaseType { get; set; }
        public override List<TypeBase> GenericArguments { get; set; }
        public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        public override TypeKind TypeKind { get; set; }
        public override List<TypeBase> ImplementedInterfaces { get; set; }
        public override List<TypeBase> NestedTypes { get; set; }
        public override List<PropertyBase> Properties { get; set; }
        public override TypeBase DeclaringType { get; set; }
        public override List<MethodBase> Methods { get; set; }
        public override List<MethodBase> Constructors { get; set; }
        private TypeMetadata(string typeName, string namespaceName)
        {
            this.TypeName = typeName;
            this.NamespaceName = namespaceName;
            this.IsSupplemented = false;
        }
        private TypeMetadata(string typeName, string namespaceName, List<TypeBase> genericArguments) : this(typeName, namespaceName)
        {
            this.GenericArguments = genericArguments;
        }

        public TypeMetadata(Type type)
        {
            this.TypeName = type.Name;
            this.DeclaringType = EmitDeclaringType(type.DeclaringType);
            this.Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            this.Methods = MethodMetadata.EmitMethods(type.GetMethods());
            this.NestedTypes = EmitNestedTypes(type.GetNestedTypes());
            this.ImplementedInterfaces = EmitImplements(type.GetInterfaces());
            this.GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
            this.Modifiers = EmitModifiers(type);
            this.BaseType = EmitExtends(type.BaseType);
            this.Properties = PropertyMetadata.EmitProperties(type.GetProperties());
            this.TypeKind = GetTypeKind(type);
            this.IsSupplemented = true;
        }

        public TypeMetadata(TypeBase typeBase)
        {
            TypeName = typeBase.TypeName;
            NamespaceName = typeBase.NamespaceName;
            TypeKind = typeBase.TypeKind;
            Modifiers = typeBase.Modifiers;
            GetBaseType(typeBase);
            GetDeclaringType(typeBase);
            try
            {
                BaseDictionary.typeDictionary.Add(TypeName, this);
            }
            catch (ArgumentException)
            {

            }
            Properties = new List<PropertyBase>();
            FillProperties(typeBase);
            Constructors = new List<MethodBase>();
            FillConstructors(typeBase);
            Methods = new List<MethodBase>();
            FillMethods(typeBase);
            GenericArguments = new List<TypeBase>();
            FillGenericArguments(typeBase);
            NestedTypes = new List<TypeBase>();
            FillNestedTypes(typeBase);
            ImplementedInterfaces = new List<TypeBase>();
            FillImplementedInterfaces(typeBase);
        }

        private void FillImplementedInterfaces(TypeBase typeBase)
        {
            if (typeBase.ImplementedInterfaces != null)
            {
                foreach (TypeBase type in typeBase.ImplementedInterfaces)
                {
                    if (BaseDictionary.typeDictionary.ContainsKey(type.TypeName))
                    {
                        ImplementedInterfaces.Add(BaseDictionary.typeDictionary[type.TypeName]);
                    }
                    else
                    {
                        ImplementedInterfaces.Add(new TypeMetadata(type));
                    }
                }
            }
        }

        private void FillNestedTypes(TypeBase typeBase)
        {
            if (typeBase.NestedTypes != null)
            {
                foreach (TypeBase type in typeBase.NestedTypes)
                {
                    if (BaseDictionary.typeDictionary.ContainsKey(type.TypeName))
                    {
                        NestedTypes.Add(BaseDictionary.typeDictionary[type.TypeName]);
                    }
                    else
                    {
                        NestedTypes.Add(new TypeMetadata(type));
                    }
                }
            }
        }

        private void FillGenericArguments(TypeBase typeBase)
        {
            if (typeBase.GenericArguments != null)
            {
                foreach (TypeBase type in typeBase.GenericArguments)
                {
                    if (BaseDictionary.typeDictionary.ContainsKey(type.TypeName))
                    {
                        GenericArguments.Add(BaseDictionary.typeDictionary[type.TypeName]);
                    }
                    else
                    {
                        GenericArguments.Add(new TypeMetadata(type));
                    }
                }
            }
        }

        private void FillMethods(TypeBase typeBase)
        {
            if (typeBase.Methods != null)
            {
                foreach (MethodBase method in typeBase.Methods)
                {
                    Methods.Add(new MethodMetadata(method));
                }
            }
        }

        private void FillConstructors(TypeBase typeBase)
        {
            if (typeBase.Constructors != null)
            {
                foreach (MethodBase constructor in typeBase.Constructors)
                {
                    Constructors.Add(new MethodMetadata(constructor));
                }
            }
        }

        private void FillProperties(TypeBase typeBase)
        {
            if (typeBase.Properties != null)
            {
                foreach (PropertyBase properties in typeBase.Properties)
                {
                    if (BaseDictionary.propertyDictionary.ContainsKey(properties.Name))
                    {
                        Properties.Add(BaseDictionary.propertyDictionary[properties.Name]);
                    }
                    else
                    {
                        BaseDictionary.propertyDictionary.Add(properties.Name, null);
                        PropertyBase newProperty = new PropertyMetadata(properties);
                        Properties.Add(newProperty);
                        BaseDictionary.propertyDictionary[newProperty.Name] = newProperty;
                    }
                }
            }
        }

        private void GetBaseType(TypeBase typeBase)
        {
            if (typeBase.BaseType != null)
            {
                if (BaseDictionary.typeDictionary.ContainsKey(typeBase.BaseType.TypeName))
                {
                    BaseType = BaseDictionary.typeDictionary[typeBase.BaseType.TypeName];
                }
                else
                {
                    BaseType = new TypeMetadata(typeBase.BaseType);
                }
            }
            else
            {
                BaseType = null;
            }
        }

        private void GetDeclaringType(TypeBase typeBase)
        {
            if (typeBase.DeclaringType != null)
            {
                if (BaseDictionary.typeDictionary.ContainsKey(typeBase.DeclaringType.TypeName))
                {
                    DeclaringType = BaseDictionary.typeDictionary[typeBase.DeclaringType.TypeName];
                }
                else
                {
                    DeclaringType = new TypeMetadata(typeBase.DeclaringType);
                }
            }
            else
            {
                DeclaringType = null;
            }
        }
        public static TypeBase EmitReference(Type type)
        {
            if (!type.IsGenericType)
            {
                long id = AssemblyLoader.idGenerator.GetId(type, out bool firstTime);
                if (firstTime)
                {
                    TypeMetadata newTypeMetadata = new TypeMetadata(type.Name, type.GetNamespace());
                    AssemblyLoader.loadedTypes.Add(id, newTypeMetadata);
                    return newTypeMetadata;
                }
                else
                {
                    bool load = AssemblyLoader.loadedTypes.TryGetValue(id, out TypeMetadata newTypeMetadata);
                    return newTypeMetadata;
                }
            }
            else
            {
                return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments()));
            }
        }

        public static void FillInTypeMetadata(Type type, TypeMetadata typeMetadata)
        {
            typeMetadata.TypeName = type.Name;
            typeMetadata.DeclaringType = EmitDeclaringType(type.DeclaringType);
            typeMetadata.Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            typeMetadata.Methods = MethodMetadata.EmitMethods(type.GetMethods());
            typeMetadata.NestedTypes = EmitNestedTypes(type.GetNestedTypes());
            typeMetadata.ImplementedInterfaces = EmitImplements(type.GetInterfaces());
            typeMetadata.GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
            typeMetadata.Modifiers = EmitModifiers(type);
            typeMetadata.BaseType = EmitExtends(type.BaseType);
            typeMetadata.Properties = PropertyMetadata.EmitProperties(type.GetProperties());
            typeMetadata.TypeKind = GetTypeKind(type);
            typeMetadata.IsSupplemented = true;
        }
        public static List<TypeBase> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return (from Type argument in arguments select EmitReference(argument)).ToList();
        }
        public bool GetSupplemented()
        {
            return this.IsSupplemented;
        }
        private static TypeBase EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private static List<TypeBase> EmitNestedTypes(IEnumerable<Type> nestedTypes)
        {
            List<TypeMetadata> typeMetadatas = (from type in nestedTypes
                where type.GetVisible()
                select new TypeMetadata(type)).ToList();
            List<TypeBase> typeBases = new List<TypeBase>();
            foreach (TypeMetadata type in typeMetadatas)
            {
                typeBases.Add(type);
            }

            return typeBases;
        }
        private static List<TypeBase> EmitImplements(IEnumerable<Type> interfaces)
        {
            return (from currentInterface in interfaces
                    select EmitReference(currentInterface)).ToList();
        }
        private static TypeKind GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKind.EnumType :
                type.IsValueType ? TypeKind.StructType :
                type.IsInterface ? TypeKind.InterfaceType :
                TypeKind.ClassType;
        }
        private static TypeBase EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }
        public static Tuple<AccessLevel, SealedEnum, AbstractEnum> EmitModifiers(Type type)
        {
            AccessLevel access = AccessLevel.IsPrivate;
            AbstractEnum abstractEnum = AbstractEnum.NotAbstract;
            SealedEnum sealedEnum = SealedEnum.NotSealed;
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
    }
}
