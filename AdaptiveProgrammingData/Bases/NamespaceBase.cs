using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingData.Bases
{
    [DataContract(IsReference = true)]
    public abstract class NamespaceBase
    {
        [DataMember]
        public virtual string NamespaceName { get; set; }
        [DataMember]
        public virtual List<TypeBase> Types { get; set; }
    }
}