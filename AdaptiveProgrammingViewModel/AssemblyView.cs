using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public class AssemblyView: TreeViewItem
    {
        private AssemblyMetadata assemblyMetadata;
        public void initializeAssembly(AssemblyMetadata assemblyMetadata)
        {
            Name = assemblyMetadata.AssemblyName + " ASSEMBLY";
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