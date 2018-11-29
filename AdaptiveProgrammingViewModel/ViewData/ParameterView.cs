﻿namespace AdaptiveProgrammingModel
{
    public class ParameterView : TreeViewItem
    {
        private ParameterMetadata parameterMetadata;
        public ParameterView(ParameterMetadata parameterMetadata)
        {
            Name = "(parameter) " + parameterMetadata.TypeMetadata.TypeName + " " + parameterMetadata.Name;
            this.parameterMetadata = parameterMetadata;
        }
        public override void BuildMyself()
        {
            if (parameterMetadata.TypeMetadata.NamespaceName != "System")
                Children.Add(new TypeView(parameterMetadata.TypeMetadata));
        }
    }
}