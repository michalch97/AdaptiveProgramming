using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingModel
{
    public class NamespaceMetadata
    {
        private string namespaceName;
        private List<TypeMetadata> typesMetadata = new List<TypeMetadata>();
        private IEnumerable<Type> t;
        public NamespaceMetadata(string namespaceName, IEnumerable<Type> types)
        {
            this.namespaceName = namespaceName;
            //this.types = from type in types orderby type.Name select new TypeMetadata(type);
            this.t = from type in types orderby type.Name select type;
            foreach (Type type in t)
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
        }

        public List<TypeMetadata> TypesMetadata
        {
            get { return typesMetadata; }
        }
    }
}