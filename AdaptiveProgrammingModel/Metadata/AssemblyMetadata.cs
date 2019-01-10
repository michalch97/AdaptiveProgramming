using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingModel
{
    public class AssemblyMetadata : AssemblyBase
    {
        public override string AssemblyName { get; set; }
        public override List<NamespaceBase> Namespaces { get; set; }

        public AssemblyMetadata(Assembly assembly)
        {
            AssemblyName = assembly.ManifestModule.Name;
            Namespaces = new List<NamespaceBase>();
            List<NamespaceMetadata> n = (from Type t in assembly.GetTypes()
                //where t.GetVisible()
                group t by t.GetNamespace()
                into g
                orderby g.Key
                select new NamespaceMetadata(g.Key, g)).ToList();
            foreach (NamespaceMetadata nm in n)
            {
                Namespaces.Add(nm);
            }
        }

        public AssemblyMetadata(AssemblyBase assemblyBase)
        {
            Namespaces = new List<NamespaceBase>();
            BaseDictionary.typeDictionary.Clear();
            BaseDictionary.propertyDictionary.Clear();
            AssemblyName = assemblyBase.AssemblyName;
            foreach (NamespaceBase namespaceBase in assemblyBase.Namespaces)
            {
                Namespaces.Add(new NamespaceMetadata(namespaceBase));
            }
        }
    }
}