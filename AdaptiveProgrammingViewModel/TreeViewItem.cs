using System.Collections.ObjectModel;

namespace AdaptiveProgrammingViewModel
{
    public class TreeViewItem//zamiast new TreeViewItem dajemy klasę, namespace, ..., dziedziczące z TreeViewItem
    {
        private bool isExpanded;
        private bool wasBuilt;
        public string Name { get; set; }
        public ObservableCollection<TreeViewItem> Children { get; set; }

        public bool IsExpanded
        {
            get { return IsExpanded; }
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

        private void BuildMyself()
        {
            //czytamy z modelu skladowe klasy którą wczytaliśmy w modelu

        }
    }
}