namespace Core.Libraries.PythonNet.Runtimes
{
    public class RuntimeShutdownException : FinalizationException
    {
        public RuntimeShutdownException(IntPtr disposable) : base("Python runtime was shut down after this object was created." +
                   " It is an error to attempt to dispose or to continue using it even after restarting the runtime.", disposable)
        {}
    }
}
