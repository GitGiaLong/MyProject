namespace Core.Libraries.PythonNet
{
    public class InternalPythonnetException : Exception
    {
        public InternalPythonnetException(string message, Exception innerException) : base(message, innerException) { }
    }
}
