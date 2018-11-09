using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingModel
{
    [Serializable]
    public class PropertyMetadata : ISerializable
    {
        private string name;
        private TypeMetadata typeMetadata;

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            name = propertyName;
            typeMetadata = propertyType;
        }
        public static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return from prop in props
                   where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                   select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType));
        }
        public string Name
        {
            get { return this.name + " (TYPE: " + typeMetadata.TypeName + ") "; }
            private set { this.name = value; }
        }
        public TypeMetadata TypeMetadata
        {
            get { return this.typeMetadata; }
            private set { this.typeMetadata = value; }
        }

        public PropertyMetadata(SerializationInfo info, StreamingContext context)
        {
            name = (string) info.GetValue("name", typeof(string));
            typeMetadata = (TypeMetadata) info.GetValue("typeMetadata", typeof(TypeMetadata));
            //Name = (string) info.GetValue("Name", typeof(string));
            //TypeMetadata = (TypeMetadata)info.GetValue("TypeMetadata", typeof(TypeMetadata));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name",name);
            info.AddValue("typeMetadata", typeMetadata);
            //info.AddValue("Name",Name);
            //info.AddValue("TypeMetadata", TypeMetadata);
        }
    }
}