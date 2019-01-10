using System;
using System.CodeDom;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    [DataContract(IsReference = true)]
    public class ParameterMetadata : ParameterBase
    {
        public override string Name { get; set; }
        public override TypeBase Type { get; set; }

        public ParameterMetadata(string name, TypeBase typeMetadata)
        {
            this.Name = name;
            this.Type = typeMetadata;
        }
        public ParameterMetadata(ParameterBase parameterBase)
        {
            Name = parameterBase.Name;
            if (BaseDictionary.typeDictionary.ContainsKey(parameterBase.Type.TypeName))
            {
                Type = BaseDictionary.typeDictionary[parameterBase.Type.TypeName];
            }
            else
            {
                Type = new TypeMetadata(parameterBase.Type);
            }
        }
    }
}