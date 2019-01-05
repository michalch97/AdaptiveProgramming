using System;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingData.Bases
{
    [DataContract(IsReference = true)]
    public abstract class PropertyBase
    {
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual TypeBase Type { get; set; }
        [DataMember]
        public virtual Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
    }
}