using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class ParameterXML : ParameterBase
    {
        [DataMember]
        public override string Name { get; set; }
        [DataMember]
        public override TypeBase Type { get; set; }

        public ParameterXML(ParameterBase parameterBase)
        {
            Name = parameterBase.Name;
            if (BaseDictionary.typeDictionary.ContainsKey(parameterBase.Type.TypeName))
            {
                Type = BaseDictionary.typeDictionary[parameterBase.Type.TypeName];
            }
            else
            {
                Type = new TypeXML(parameterBase.Type);
            }
        }

    }
}