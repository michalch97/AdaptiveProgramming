using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    public class PropertyMetadata : PropertyBase
    {
        public override string Name { get; set; }
        public override TypeBase Type { get; set; }
        public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }

        public PropertyMetadata(string name, TypeBase typeMetadata,
            Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers)
        {
            this.Name = name;
            this.Type = typeMetadata;
            this.Modifiers = modifiers;
        }

        public PropertyMetadata(PropertyBase propertyBase)
        {
            Name = propertyBase.Name;
            Modifiers = propertyBase.Modifiers;
            if (BaseDictionary.typeDictionary.ContainsKey(propertyBase.Type.TypeName))
            {
                Type = BaseDictionary.typeDictionary[propertyBase.Type.TypeName];
            }
            else
            {
                BaseDictionary.typeDictionary.Add(propertyBase.Type.TypeName,null);
                Type = new TypeMetadata(propertyBase.Type);
                BaseDictionary.typeDictionary[Type.TypeName] = Type;
            }
        }

        public static List<PropertyBase> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            List<PropertyMetadata> propertyMetadatas = (from prop in props
                where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType), TypeMetadata.EmitModifiers(prop.PropertyType))).ToList();
            List<PropertyBase> propertyBases = new List<PropertyBase>();
            foreach (PropertyMetadata property in propertyMetadatas)
            {
                propertyBases.Add(property);
            }

            return propertyBases;
            //return (from prop in props
            //        where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
            //        select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType),TypeMetadata.EmitModifiers(prop.PropertyType))).ToList();
        }
    }
}