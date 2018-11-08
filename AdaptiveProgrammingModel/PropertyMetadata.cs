using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdaptiveProgrammingModel
{
    public class PropertyMetadata
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
            get { return this.name + " (TYPE: " + typeMetadata.TypeName + ") " ; }
        }

        public TypeMetadata TypeMetadata
        {
            get { return this.typeMetadata; }
        }
    }
}