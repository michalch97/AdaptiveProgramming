using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    public class PropertyMetadata
    {
        private string name;
        private TypeMetadata typeMetadata;
        private Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers;
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }
        public TypeMetadata TypeMetadata
        {
            get { return this.typeMetadata; }
            private set { this.typeMetadata = value; }
        }
        public Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers
        {
            get { return this.modifiers; }
            private set { this.modifiers = value; }
        }

        [JsonConstructor]
        public PropertyMetadata(string name, TypeMetadata typeMetadata,
            Tuple<AccessLevel, SealedEnum, AbstractEnum> modifiers)
        {
            this.Name = name;
            this.TypeMetadata = typeMetadata;
            this.Modifiers = modifiers;
        }

        public static List<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return (from prop in props
                    where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                    select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType),TypeMetadata.EmitModifiers(prop.PropertyType))).ToList();
        }
    }
}