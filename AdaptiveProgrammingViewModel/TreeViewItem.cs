using System.Collections.ObjectModel;
using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public abstract class TreeViewItem//zamiast new TreeViewItem dajemy klasę, namespace, ..., dziedziczące z TreeViewItem
    {
        private bool isExpanded;
        public bool wasBuilt;
        public string Name { get; set; }
        public ObservableCollection<TreeViewItem> Children { get; set; } = new ObservableCollection<TreeViewItem>();

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                if (wasBuilt)
                {
                    return;
                }
                Children.Clear();
                BuildMyself();
                wasBuilt = true;
            }
        }

        public abstract void BuildMyself();
    }
}