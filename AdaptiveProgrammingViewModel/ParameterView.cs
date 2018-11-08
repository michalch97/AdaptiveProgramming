using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public class ParameterView : TreeViewItem
    {
        public ParameterView(ParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name + "PARAMETER";
        }
        public override void BuildMyself()
        {
            return;
        }
    }
}