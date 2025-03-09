using Core.Libraries.WPF.Interactivities.Enums;

namespace Core.Libraries.WPF.Interactivities
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
