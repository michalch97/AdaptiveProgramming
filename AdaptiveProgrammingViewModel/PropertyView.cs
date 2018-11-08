using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public class PropertyView : TreeViewItem
    {
        public PropertyView(PropertyMetadata propertyMetadata)
        {
            Name = propertyMetadata.Name + "PROPERTY";
        }
        public override void BuildMyself()
        {
            return;
        }
    }
}