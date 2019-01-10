using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingModel
{
    public class NamespaceMetadata : NamespaceBase
    {
        private List<Type> tp;
        public override string NamespaceName { get; set; }
        public override List<TypeBase> Types { get; set; }

        public List<Type> Tp
        {
            get { return this.tp; }
            private set { this.tp = value; }
        }

        public NamespaceMetadata(string namespaceName, IEnumerable<Type> types)
        {
            Types = new List<TypeBase>();
            this.NamespaceName = namespaceName;
            this.Tp = (from type in types orderby type.Name select type).ToList();
            foreach (Type type in Tp)
            {
                long id = AssemblyLoader.idGenerator.GetId(type, out bool firsTime);
                if (firsTime)
                {
                    TypeMetadata newTypeMetadata = new TypeMetadata(type);
                    Types.Add(newTypeMetadata);
                    AssemblyLoader.loadedTypes.Add(id, newTypeMetadata);
                }
                else
                {
                    AssemblyLoader.loadedTypes.TryGetValue(id, out TypeMetadata loadedTypeMetadata);
                    TypeMetadata.FillInTypeMetadata(type,loadedTypeMetadata);
                    Types.Add(loadedTypeMetadata);
                }
            }
        }

        public NamespaceMetadata(NamespaceBase namespaceBase)
        {
            Types = new List<TypeBase>();
            NamespaceName = namespaceBase.NamespaceName;
            foreach (TypeBase type in namespaceBase.Types)
            {
                Types.Add(new TypeMetadata(type));
            }
        }
    }
}