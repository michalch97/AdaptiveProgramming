using System;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class PropertyXML : PropertyBase
    {
        [DataMember]
        public override string Name { get; set; }
        [DataMember]
        public override TypeBase Type { get; set; }
        [DataMember]
        public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }

        public PropertyXML(PropertyBase propertyBase)
        {
            Name = propertyBase.Name;
            Modifiers = propertyBase.Modifiers;
            if (BaseDictionary.typeDictionary.ContainsKey(propertyBase.Type.TypeName))
            {
                Type = BaseDictionary.typeDictionary[propertyBase.Type.TypeName];
            }
            else
            {
                BaseDictionary.typeDictionary.Add(propertyBase.Type.TypeName, null);
                Type = new TypeXML(propertyBase.Type);
                BaseDictionary.typeDictionary[Type.TypeName] = Type;
            }
        }
    }
}