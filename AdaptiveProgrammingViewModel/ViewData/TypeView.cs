using System.Collections;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingModel
{
    public class TypeView : TreeViewItem
    {
        private TypeMetadata typeMetadata;
        public TypeView(TypeMetadata typeMetadata)
        {
            string interfaces = " : ";
            if (typeMetadata.ImplementedInterfaces != null)
            {
                foreach (TypeMetadata interfaceMetadata in typeMetadata.ImplementedInterfaces)
                {
                    interfaces += (interfaceMetadata.TypeName + ", ");
                }
            }

            string before = "";
            if (typeMetadata.Modifiers != null)
            {
                switch (typeMetadata.Modifiers.Item1)
                {
                    case AccessLevel.IsPrivate:
                        before += "private ";
                        break;
                    case AccessLevel.IsProtected:
                        before += "protected ";
                        break;
                    case AccessLevel.IsProtectedInternal:
                        before += "internal ";
                        break;
                    case AccessLevel.IsPublic:
                        before += "public ";
                        break;
                }
            
                switch (typeMetadata.Modifiers.Item3)
                {
                    case AbstractEnum.Abstract:
                        before += "abstract ";
                        break;
                }
            }

            if (typeMetadata.TypeKind!=null)
            {
                switch (typeMetadata.TypeKind)
                {
                    case TypeKind.ClassType:
                        before += "class ";
                        break;
                    case TypeKind.InterfaceType:
                        before += "interface ";
                        break;
                    case TypeKind.EnumType:
                        before += "enum ";
                        break;
                }
            }
            

            Name = before + typeMetadata.TypeName + (interfaces.Length != 3 ? interfaces : null);
            this.typeMetadata = typeMetadata;
        }
        public override void BuildMyself()
        {
            if (typeMetadata.Constructors != null)
            {
                foreach (MethodMetadata constructorMetadata in typeMetadata.Constructors)
                {
                    Children.Add(new MethodView(constructorMetadata, typeMetadata.TypeName));
                }
                foreach (TypeMetadata nestedTypeMetadata in typeMetadata.NestedTypes)
                {
                    Children.Add(new TypeView(nestedTypeMetadata));
                }
            }

            if (typeMetadata.Methods != null)
            {
                foreach (MethodMetadata methodMetadata in typeMetadata.Methods)
                {
                    Children.Add(new MethodView(methodMetadata, typeMetadata.TypeName));
                }
            }

            if (typeMetadata.Properties != null)
            {
                foreach (PropertyMetadata propertyMetadata in typeMetadata.Properties)
                {
                    Children.Add(new PropertyView(propertyMetadata));
                }
            }
        }
    }
}