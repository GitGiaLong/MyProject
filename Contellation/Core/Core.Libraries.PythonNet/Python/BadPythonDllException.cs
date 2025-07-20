namespace Core.Libraries.PythonNet.Python
{
    internal class BadPythonDllException : MissingMethodException
    {
        public BadPythonDllException(string message, Exception innerException) : base(message, innerException) { }
    }
}
