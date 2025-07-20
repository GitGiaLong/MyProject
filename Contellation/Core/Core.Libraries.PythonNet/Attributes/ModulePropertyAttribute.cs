namespace Core.Libraries.PythonNet.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    internal class ModulePropertyAttribute : Attribute
    {
        public ModulePropertyAttribute() { }
    }
}
