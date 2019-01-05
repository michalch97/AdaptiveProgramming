using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    public class AssemblyMetadata
    {
        internal string assemblyName;
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
    }
}