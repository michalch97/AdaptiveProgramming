using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace AdaptiveProgrammingModel
{
    public class AssemblyMetadata
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
        }

        public IEnumerable<NamespaceMetadata> Namespaces
        {
            get { return namespaces; }
        }
    }
}