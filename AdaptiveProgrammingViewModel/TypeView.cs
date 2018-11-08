using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public class TypeView : TreeViewItem
    {
        private TypeMetadata typeMetadata;
        public TypeView(TypeMetadata typeMetadata)
        {
            Name = typeMetadata.TypeName + " CLASS/TYPE";
            this.typeMetadata = typeMetadata;
        }
        public override void BuildMyself()
        {
            foreach (MethodMetadata methodMetadata in typeMetadata.MethodsMetadata)
            {
                Children.Add(new MethodView(methodMetadata));
            }
            foreach (PropertyMetadata propertyMetadata in typeMetadata.Properties)
            {
                Children.Add(new PropertyView(propertyMetadata));
            }
            foreach (TypeMetadata nestedTypeMetadata in typeMetadata.NestedTypesMetadata)
            {
                Children.Add(new TypeView(nestedTypeMetadata));
            }
        }
    }
}