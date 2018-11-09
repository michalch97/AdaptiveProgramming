using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingModel
{
    [Serializable]
    public class NamespaceMetadata : ISerializable
    {
        private string namespaceName;
        private List<TypeMetadata> typesMetadata = new List<TypeMetadata>();
        private List<Type> tp = new List<Type>();
        
        public NamespaceMetadata(string namespaceName, IEnumerable<Type> types)
        {
            this.namespaceName = namespaceName;
            this.tp = (from type in types orderby type.Name select type).ToList();
            foreach (Type type in tp)
            {
                long id = AssemblyLoader.idGenerator.GetId(type, out bool firsTime);
                if (firsTime)
                {
                    TypeMetadata newTypeMetadata = new TypeMetadata(type);
                    typesMetadata.Add(newTypeMetadata);
                    AssemblyLoader.loadedTypes.Add(id, newTypeMetadata);
                }
                else
                {
                    AssemblyLoader.loadedTypes.TryGetValue(id, out TypeMetadata loadedTypeMetadata);
                    typesMetadata.Add(loadedTypeMetadata);
                }
            }
        }
        public string NamespaceName
        {
            get { return this.namespaceName; }
            private set { this.namespaceName = value; }
        }
        public List<TypeMetadata> TypesMetadata
        {
            get { return this.typesMetadata; }
            private set { this.typesMetadata = value; }
        }

        public NamespaceMetadata(SerializationInfo info, StreamingContext context)
        {
            namespaceName = (string) info.GetValue("namespaceName", typeof(string));
            typesMetadata = (List<TypeMetadata>) info.GetValue("typesMetadata", typeof(List<TypeMetadata>));
            IEnumerable<string> temptp = (IEnumerable<string>) info.GetValue("tp", typeof(IEnumerable<string>));
            foreach (string s in temptp)
            {
                tp.Add(Type.GetType(s));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("namespaceName", namespaceName);
            info.AddValue("typesMetadata", typesMetadata);
            info.AddValue("tp", tp);
        }
    }
}