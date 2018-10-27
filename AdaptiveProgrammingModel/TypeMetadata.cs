using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveProgrammingModel
{
    public class TypeMetadata
    {
        private string typeName;
        private string namespaceName;
        private TypeMetadata baseType;
        private IEnumerable<TypeMetadata> genericArguments;
        private Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers;
        private TypeKind typeKind;
        private IEnumerable<Attribute> attributes;
        private IEnumerable<TypeMetadata> implementedInterfaces;
        private IEnumerable<TypeMetadata>nestedTypes;
        private IEnumerable<PropertyMetadata> properties;
        private TypeMetadata declaringType;
        private IEnumerable<MethodMetadata> methods;
        private IEnumerable<MethodMetadata> constructors;

        private TypeMetadata(string typeName, string namespaceName)
        {
            this.typeName = typeName;
            this.namespaceName = namespaceName;
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
        }

        public static TypeMetadata EmitReference(Type type)
        {
            if (!type.IsGenericType)
                return new TypeMetadata(type.Name, type.GetNamespace());
            else
                return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments()));
        }
        public static IEnumerable<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return from Type argument in arguments select EmitReference(argument);
        }
        private TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private IEnumerable<TypeMetadata> EmitNestedTypes(IEnumerable<Type> nestedTypes)
        {
            return from type in nestedTypes
                where type.GetVisible()
                select new TypeMetadata(type);
        }
        private IEnumerable<TypeMetadata> EmitImplements(IEnumerable<Type> interfaces)
        {
            return from currentInterface in interfaces
                select EmitReference(currentInterface);
        }
        private static TypeKind GetTypeKind(Type type) //#80 TPA: Reflection - Invalid return value of GetTypeKind() 
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
    }
}
