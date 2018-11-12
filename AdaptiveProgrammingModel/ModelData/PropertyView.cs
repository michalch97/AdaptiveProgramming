namespace AdaptiveProgrammingModel
{
    public class PropertyView : TreeViewItem
    {
        private PropertyMetadata propertyMetadata;
        public PropertyView(PropertyMetadata propertyMetadata)
        {
            string before = "";
            switch (propertyMetadata.Modifiers.Item1)
            {
                case AccessLevel.IsPrivate:
                    before += "private ";
                    break;
                case AccessLevel.IsProtected:
                    before += "protected ";
                    break;
                case AccessLevel.IsProtectedInternal:
                    before += "internal ";
                    break;
                case AccessLevel.IsPublic:
                    before += "public ";
                    break;
            }
            switch (propertyMetadata.Modifiers.Item3)
            {
                case AbstractEnum.Abstract:
                    before += "abstract ";
                    break;
            }
            Name = "(property) " + before + propertyMetadata.TypeMetadata.TypeName + " " + propertyMetadata.Name;
            this.propertyMetadata = propertyMetadata;
        }
        public override void BuildMyself()
        {
            return;
        }
    }
}