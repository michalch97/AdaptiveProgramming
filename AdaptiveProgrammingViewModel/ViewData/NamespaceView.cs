using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingModel
{
    public class NamespaceView : TreeViewItem
    {
        private NamespaceMetadata namespaceMetadata;
        public NamespaceView(NamespaceMetadata namespaceMetadata)
        {
            Name = "namespace " + namespaceMetadata.NamespaceName;
            this.namespaceMetadata = namespaceMetadata;
        }
        public override void BuildMyself()
        {
            foreach (TypeMetadata typeMetadata in namespaceMetadata.Types)
            {
                Children.Add(new TypeView(typeMetadata));
            }
        }
    }
}