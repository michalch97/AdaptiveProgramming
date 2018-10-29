using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdaptiveProgrammingModel
{
    public class NamespaceMetadata
    {
        private string namespaceName;
        private IEnumerable<TypeMetadata> types;

        public NamespaceMetadata(string namespaceName, IEnumerable<Type> types)
        {
            this.namespaceName = namespaceName;
            this.types = from type in types orderby type.Name select new TypeMetadata(type);
        }
    }
}