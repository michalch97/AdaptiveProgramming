using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class MethodXML : MethodBase
    {
        [DataMember]
        public override string Name { get; set; }
        [DataMember]
        public override List<TypeBase> GenericArguments { get; set; }
        [DataMember]
        public override Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        [DataMember]
        public override TypeBase ReturnType { get; set; }
        [DataMember]
        public override bool Extension { get; set; }
        [DataMember]
        public override List<ParameterBase> Parameters { get; set; }
    }
}