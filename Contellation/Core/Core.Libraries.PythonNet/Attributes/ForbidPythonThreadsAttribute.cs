namespace Core.Libraries.PythonNet.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    internal class ForbidPythonThreadsAttribute : Attribute
    {
        public ForbidPythonThreadsAttribute() { }
    }
}
