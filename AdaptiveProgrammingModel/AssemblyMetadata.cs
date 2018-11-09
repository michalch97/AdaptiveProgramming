using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingModel
{
    [Serializable]
    public class AssemblyMetadata : ISerializable
    {
        internal string assemblyName;
        internal IEnumerable<NamespaceMetadata> namespaces;
        
        public AssemblyMetadata(Assembly assembly)
        {
            assemblyName = assembly.ManifestModule.Name;
            namespaces = from Type t in assembly.GetTypes()
                             //where t.GetVisible()
                         group t by t.GetNamespace()
                         into g
                         orderby g.Key
                         select new NamespaceMetadata(g.Key, g);
        }
        
        public string AssemblyName
        {
            get { return assemblyName; }
            private set { this.assemblyName = value; }
        }
        public IEnumerable<NamespaceMetadata> Namespaces
        {
            get { return namespaces; }
            private set { this.namespaces = value; }
        }
        public AssemblyMetadata(SerializationInfo info, StreamingContext context)
        {
            assemblyName = (string) info.GetValue("assemblyName", typeof(string));
            namespaces =
                (IEnumerable<NamespaceMetadata>) info.GetValue("namespaces", typeof(IEnumerable<NamespaceMetadata>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("assemblyName",assemblyName);
            info.AddValue("namespaces",namespaces);
        }
    }
}