using System.Collections.Generic;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class NamespaceXML : NamespaceBase
    {
        [DataMember]
        public override string NamespaceName { get; set; }
        [DataMember]
        public override List<TypeBase> Types { get; set; }
    }
}