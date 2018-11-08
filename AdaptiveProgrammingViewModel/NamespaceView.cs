using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public class NamespaceView : TreeViewItem
    {
        private NamespaceMetadata namespaceMetadata;
        public NamespaceView(NamespaceMetadata namespaceMetadata)
        {
            Name = namespaceMetadata.NamespaceName + " NAMESPACE";
            this.namespaceMetadata = namespaceMetadata;
        }
        public override void BuildMyself()
        {
            foreach (TypeMetadata typeMetadata in namespaceMetadata.TypesMetadata)
            {
                Children.Add(new TypeView(typeMetadata));
            }
        }
    }
}