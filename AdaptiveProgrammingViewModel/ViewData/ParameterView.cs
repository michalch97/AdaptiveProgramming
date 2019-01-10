namespace AdaptiveProgrammingModel
{
    public class ParameterView : TreeViewItem
    {
        private ParameterMetadata parameterMetadata;
        public ParameterView(ParameterMetadata parameterMetadata)
        {
            Name = "(parameter) " + parameterMetadata.Type.TypeName + " " + parameterMetadata.Name;
            this.parameterMetadata = parameterMetadata;
        }
        public override void BuildMyself()
        {
            if (parameterMetadata.Type.NamespaceName != "System")
                Children.Add(new TypeView((TypeMetadata)parameterMetadata.Type));
        }
    }
}