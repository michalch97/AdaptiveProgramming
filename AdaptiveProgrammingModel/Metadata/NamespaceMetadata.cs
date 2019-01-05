using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    [DataContract(IsReference = true)]
    public class NamespaceMetadata
    {
        [DataMember]
        private string namespaceName;
        [DataMember]
        private List<TypeMetadata> typesMetadata;// = new List<TypeMetadata>();
        private List<Type> tp;// = new List<Type>();
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

        public List<Type> Tp
        {
            get { return this.tp; }
            private set { this.tp = value; }
        }

        public NamespaceMetadata(string namespaceName, IEnumerable<Type> types)
        {
            TypesMetadata = new List<TypeMetadata>();
            this.NamespaceName = namespaceName;
            this.Tp = (from type in types orderby type.Name select type).ToList();
            foreach (Type type in Tp)
            {
                long id = AssemblyLoader.idGenerator.GetId(type, out bool firsTime);
                if (firsTime)
                {
                    TypeMetadata newTypeMetadata = new TypeMetadata(type);
                    TypesMetadata.Add(newTypeMetadata);
                    AssemblyLoader.loadedTypes.Add(id, newTypeMetadata);
                }
                else
                {
                    AssemblyLoader.loadedTypes.TryGetValue(id, out TypeMetadata loadedTypeMetadata);
                    TypeMetadata.FillInTypeMetadata(type,loadedTypeMetadata);
                    TypesMetadata.Add(loadedTypeMetadata);
                }
            }
        }

        public ISerializableNamespace GetSerializableNamespace()
        {
            
        }
    }
}