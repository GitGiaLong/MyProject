using Libraries.Custom.Interactivities.Enums;

namespace Libraries.Custom.Interactivities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CustomPropertyValueEditorAttribute : Attribute
    {
        public CustomPropertyValueEditor CustomPropertyValueEditor { get; private set; }
        public CustomPropertyValueEditorAttribute(CustomPropertyValueEditor customPropertyValueEditor)
        {
            CustomPropertyValueEditor = customPropertyValueEditor;
        }
    }
}
