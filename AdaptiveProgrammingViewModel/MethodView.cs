using System;
using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel
{
    public class MethodView : TreeViewItem
    {
        private MethodMetadata methodMetadata;
        public MethodView(MethodMetadata methodMetadata)
        {
            Name = methodMetadata.Name + "METHOD";
            this.methodMetadata = methodMetadata;
        }
        public override void BuildMyself()
        {
            foreach (ParameterMetadata parameterMetadata in methodMetadata.Parameters)
            {
                Children.Add(new ParameterView(parameterMetadata));
            }
        }
    }
}