using System;
using System.CodeDom;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    [DataContract(IsReference = true)]
    public class ParameterMetadata
    {
        [DataMember]
        private string name;
        [DataMember]
        private TypeMetadata typeMetadata;
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }
        public TypeMetadata TypeMetadata
        {
            get { return this.typeMetadata; }
            private set { this.typeMetadata = value; }
        }

        [JsonConstructor]
        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.Name = name;
            this.TypeMetadata = typeMetadata;
        }
    }
}