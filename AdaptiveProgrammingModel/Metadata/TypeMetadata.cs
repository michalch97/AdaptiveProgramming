using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    public class TypeMetadata
    {
        private bool isSupplemented;
        private string typeName;
        private string namespaceName;
        private TypeMetadata baseType;
        private List<TypeMetadata> genericArguments;
        private Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers;
        private TypeKind typeKind;
        private List<TypeMetadata> implementedInterfaces;
        private List<TypeMetadata> nestedTypes;
        private List<PropertyMetadata> properties;
        private TypeMetadata declaringType;
        private List<MethodMetadata> methods;
        private List<MethodMetadata> constructors;
        public bool IsSupplemented
        {
            get
            {
                return this.isSupplemented;
            }
            private set { this.isSupplemented = value; }
        }
        public string TypeName
        {
            get
            {
                return this.typeName;
            }
            private set { this.typeName = value; }
        }
        public string NamespaceName
        {
            get
            {
                return this.namespaceName;
            }
            private set { this.namespaceName = value; }
        }
        public TypeMetadata BaseType
        {
            get
            {
                return this.baseType;
            }
            private set { this.baseType = value; }
        }
        public List<TypeMetadata> GenericArguments
        {
            get
            {
                return this.genericArguments;
            }
            private set { this.genericArguments = value; }
        }
        public Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers
        {
            get
            {
                return this.modifiers;
            }
            private set { this.modifiers = value; }
        }
        public TypeKind TypeKind
        {
            get
            {
                return this.typeKind;
            }
            private set { this.typeKind = value; }
        }
        public List<TypeMetadata> ImplementedInterfaces
        {
            get
            {
                return this.implementedInterfaces;
            }
            private set { this.implementedInterfaces = value; }
        }
        public List<TypeMetadata> NestedTypes
        {
            get
            {
                return this.nestedTypes;
            }
            private set { this.nestedTypes = value; }
        }
        public List<PropertyMetadata> Properties
        {
            get
            {
                return this.properties;
            }
            private set { this.properties = value; }
        }
        public TypeMetadata DeclaringType
        {
            get
            {
                return this.declaringType;
            }
            private set { this.declaringType = value; }
        }
        public List<MethodMetadata> Methods
        {
            get
            {
                return this.methods;
            }
            private set { this.methods = value; }
        }
        public List<MethodMetadata> Constructors
        {
            get
            {
                return this.constructors;
            }
            private set { this.constructors = value; }
        }
        private TypeMetadata(string typeName, string namespaceName)
        {
            this.TypeName = typeName;
            this.NamespaceName = namespaceName;
            this.IsSupplemented = false;
        }
        private TypeMetadata(string typeName, string namespaceName, List<TypeMetadata> genericArguments) : this(typeName, namespaceName)
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
        [JsonConstructor]
        public TypeMetadata(bool isSupplemented, string typeName, string namespaceName, TypeMetadata baseType,
            List<TypeMetadata> genericArguments, Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers,
            TypeKind typeKind, List<TypeMetadata> implementedInterfaces,
            List<TypeMetadata> nestedTypes, List<PropertyMetadata> properties, TypeMetadata declaringType,
            List<MethodMetadata> methods, List<MethodMetadata> constructors)
        {
            this.IsSupplemented = isSupplemented;
            this.TypeName = typeName;
            this.NamespaceName = namespaceName;
            this.BaseType = baseType;
            this.GenericArguments = genericArguments;
            this.Modifiers = modifiers;
            this.TypeKind = typeKind;
            this.ImplementedInterfaces = implementedInterfaces;
            this.NestedTypes = nestedTypes;
            this.Properties = properties;
            this.DeclaringType = declaringType;
            this.Methods = methods;
            this.Constructors = constructors;
        }
        public static TypeMetadata EmitReference(Type type)
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
        public static List<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return (from Type argument in arguments select EmitReference(argument)).ToList();
        }
        public bool GetSupplemented()
        {
            return this.IsSupplemented;
        }
        private static TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private static List<TypeMetadata> EmitNestedTypes(IEnumerable<Type> nestedTypes)
        {
            return (from type in nestedTypes
                    where type.GetVisible()
                    select new TypeMetadata(type)).ToList();
        }
        private static List<TypeMetadata> EmitImplements(IEnumerable<Type> interfaces)
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
        private static TypeMetadata EmitExtends(Type baseType)
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
