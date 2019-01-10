using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class TypeXML : TypeBase
    {
        [DataMember] public override string TypeName { get; set; }
        [DataMember] public override string NamespaceName { get; set; }
        [DataMember] public override TypeBase BaseType { get; set; }
        [DataMember] public override List<TypeBase> GenericArguments { get; set; }
        [DataMember] public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        [DataMember] public override TypeKind TypeKind { get; set; }
        [DataMember] public override List<TypeBase> ImplementedInterfaces { get; set; }
        [DataMember] public override List<TypeBase> NestedTypes { get; set; }
        [DataMember] public override List<PropertyBase> Properties { get; set; }
        [DataMember] public override TypeBase DeclaringType { get; set; }
        [DataMember] public override List<MethodBase> Methods { get; set; }
        [DataMember] public override List<MethodBase> Constructors { get; set; }

        public TypeXML(TypeBase typeBase)
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
                        ImplementedInterfaces.Add(new TypeXML(type));
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
                        NestedTypes.Add(new TypeXML(type));
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
                        GenericArguments.Add(new TypeXML(type));
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
                    Methods.Add(new MethodXML(method));
                }
            }
        }

        private void FillConstructors(TypeBase typeBase)
        {
            if (typeBase.Constructors != null)
            {
                foreach (MethodBase constructor in typeBase.Constructors)
                {
                    Constructors.Add(new MethodXML(constructor));
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
                        BaseDictionary.propertyDictionary.Add(properties.Name,null);
                        PropertyBase newProperty = new PropertyXML(properties);
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
                    BaseType = new TypeXML(typeBase.BaseType);
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
                    DeclaringType = new TypeXML(typeBase.DeclaringType);
                }
            }
            else
            {
                DeclaringType = null;
            }
        }
    }
}