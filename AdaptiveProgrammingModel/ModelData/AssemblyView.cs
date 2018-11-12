namespace AdaptiveProgrammingModel
{
    public class AssemblyView: TreeViewItem
    {
        private AssemblyMetadata assemblyMetadata;
        public void initializeAssembly(AssemblyMetadata assemblyMetadata)
        {
            Name = "assembly " + assemblyMetadata.AssemblyName;
            this.assemblyMetadata = assemblyMetadata;
        }
        public override void BuildMyself()
        {
            foreach (NamespaceMetadata namespaceMetadata in assemblyMetadata.Namespaces)
            {
                Children.Add(new NamespaceView(namespaceMetadata));
            }
        }

    }
}