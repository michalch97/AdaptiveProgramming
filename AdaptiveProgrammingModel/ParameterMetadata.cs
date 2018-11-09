using System;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingModel
{
    [Serializable]
    public class ParameterMetadata : ISerializable
    {
        private string name;
        private TypeMetadata typeMetadata;
        
        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.name = name;
            this.typeMetadata = typeMetadata;
        }
        public string Name
        {
            get { return this.name + " (TYPE: " + typeMetadata.TypeName + ") "; }
            private set { this.name = value; }
        }

        public ParameterMetadata(SerializationInfo info, StreamingContext context)
        {
            name = (string) info.GetValue("name", typeof(string));
            typeMetadata = (TypeMetadata) info.GetValue("typeMetadata", typeof(TypeMetadata));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name",name);
            info.AddValue("typeMetadata", typeMetadata);
        }
    }
}