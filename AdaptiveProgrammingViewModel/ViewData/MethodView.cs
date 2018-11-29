using System;

namespace AdaptiveProgrammingModel
{
    public class MethodView : TreeViewItem
    {
        private MethodMetadata methodMetadata;
        public MethodView(MethodMetadata methodMetadata, string className)
        {
            string modifiers = "";
            switch (methodMetadata.Modifiers.Item1)
            {
                case AccessLevel.IsPrivate:
                    modifiers += "private ";
                    break;
                case AccessLevel.IsProtected:
                    modifiers += "protected ";
                    break;
                case AccessLevel.IsProtectedInternal:
                    modifiers += "internal ";
                    break;
                case AccessLevel.IsPublic:
                    modifiers += "public ";
                    break;
            }
            switch (methodMetadata.Modifiers.Item2)
            {
                case AbstractEnum.Abstract:
                    modifiers += "abstract ";
                    break;
            }
            switch (methodMetadata.Modifiers.Item3)
            {
                case StaticEnum.Static:
                    modifiers += "static ";
                    break;
            }
            switch (methodMetadata.Modifiers.Item4)
            {
                case VirtualEnum.Virtual:
                    modifiers += "virtual ";
                    break;
            }
            Name = (methodMetadata.ReturnType == null ? ("(constructor) " + modifiers + className) : ("(method) " + modifiers + methodMetadata.ReturnType.TypeName + " " + methodMetadata.Name));
            this.methodMetadata = methodMetadata;
        }
        public override void BuildMyself()
        {
            foreach (ParameterMetadata parameterMetadata in methodMetadata.Parameters)
            {
                Children.Add(new ParameterView(parameterMetadata));
            }

            if (methodMetadata.ReturnType != null && methodMetadata.ReturnType.NamespaceName != "System")
                Children.Add(new TypeView(methodMetadata.ReturnType));
        }
    }
}