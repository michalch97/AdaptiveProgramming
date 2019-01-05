using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class AssemblyXML : AssemblyBase
    {
        [DataMember]
        public override string AssemblyName { get; set; }
        [DataMember]
        public override List<NamespaceBase> Namespaces { get; set; }
    }
}