namespace Core.WPF.Interactivities.Layouts
{

    public sealed class FluidMoveSetTagBehavior : FluidMoveBehaviorBase
    {
        internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, TagData newTagData)
        {
            TagData tagData;
            bool gotData = TagDictionary.TryGetValue(tag, out tagData);

            if (!gotData)
            {
                tagData = new TagData();
                TagDictionary.Add(tag, tagData);
            }

            tagData.ParentRect = newTagData.ParentRect;
            tagData.AppRect = newTagData.AppRect;
            tagData.Parent = newTagData.Parent;
            tagData.Child = newTagData.Child;
            tagData.Timestamp = newTagData.Timestamp;
        }
    }
}
