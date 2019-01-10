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

        public AssemblyXML(AssemblyBase assemblyBase)
        {
            BaseDictionary.typeDictionary.Clear();
            BaseDictionary.propertyDictionary.Clear();
            AssemblyName = assemblyBase.AssemblyName;
            Namespaces = new List<NamespaceBase>();
            foreach (NamespaceBase namespaceBase in assemblyBase.Namespaces)
            {
                Namespaces.Add(new NamespaceXML(namespaceBase));
            }
        }
    }
}