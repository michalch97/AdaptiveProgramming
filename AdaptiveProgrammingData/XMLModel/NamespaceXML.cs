using System.Collections.Generic;
using System.Data;
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

        public NamespaceXML(NamespaceBase namespaceBase)
        {
            NamespaceName = namespaceBase.NamespaceName;
            Types = new List<TypeBase>();
            foreach (TypeBase typeBase in namespaceBase.Types)
            {
                if (BaseDictionary.typeDictionary.ContainsKey(typeBase.TypeName))
                {
                    Types.Add(BaseDictionary.typeDictionary[typeBase.TypeName]);
                }
                else
                {
                    Types.Add(new TypeXML(typeBase));
                }
            }
        }
    }
}