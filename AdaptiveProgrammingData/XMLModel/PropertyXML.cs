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
    }
}