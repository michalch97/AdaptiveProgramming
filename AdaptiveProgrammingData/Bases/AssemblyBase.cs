using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingData.Bases
{
    [DataContract(IsReference = true)]
    public abstract class AssemblyBase
    {
        [DataMember]
        public virtual string AssemblyName { get; set; }
        [DataMember]
        public virtual List<NamespaceBase> Namespaces { get; set; }
    }
}