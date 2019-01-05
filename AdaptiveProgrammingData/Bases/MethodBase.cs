using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingData.Bases
{
    [DataContract(IsReference = true)]
    public abstract class MethodBase
    {
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual List<TypeBase> GenericArguments { get; set; }
        [DataMember]
        public virtual Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        [DataMember]
        public virtual TypeBase ReturnType { get; set; }
        [DataMember]
        public virtual bool Extension { get; set; }
        [DataMember]
        public virtual List<ParameterBase> Parameters { get; set; }
    }
}