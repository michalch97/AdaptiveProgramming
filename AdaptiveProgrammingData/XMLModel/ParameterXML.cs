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
    }
}