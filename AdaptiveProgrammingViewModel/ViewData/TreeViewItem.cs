using System.Collections.ObjectModel;

namespace AdaptiveProgrammingModel
{
    public abstract class TreeViewItem
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