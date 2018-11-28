using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    [DataContract(IsReference = true)]
    public class AssemblyMetadata
    {
        [DataMember]
        internal string assemblyName;
        [DataMember]
        internal List<NamespaceMetadata> namespaces;
        public string AssemblyName
        {
            get { return assemblyName; }
            private set { this.assemblyName = value; }
        }
        public List<NamespaceMetadata> Namespaces
        {
            get { return namespaces; }
            private set { this.namespaces = value; }
        }

        public AssemblyMetadata(Assembly assembly)
        {
            AssemblyName = assembly.ManifestModule.Name;
            Namespaces = (from Type t in assembly.GetTypes()
                              //where t.GetVisible()
                          group t by t.GetNamespace()
                         into g
                          orderby g.Key
                          select new NamespaceMetadata(g.Key, g)).ToList();
        }

        [JsonConstructor]
        public AssemblyMetadata(String assemblyName, List<NamespaceMetadata> namespaces)
        {
            this.AssemblyName = assemblyName;
            this.Namespaces = namespaces;
        }
    }
}