using System.Runtime.Serialization;

namespace AdaptiveProgrammingData.Bases
{
    [DataContract(IsReference = true)]
    public abstract class ParameterBase
    {
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual TypeBase Type { get; set; }
    }
}