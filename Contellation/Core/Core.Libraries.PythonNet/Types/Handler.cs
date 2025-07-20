namespace Core.Libraries.PythonNet.Types
{
    internal class Handler
    {
        public readonly nint hash;
        public readonly Delegate del;

        public Handler(nint hash, Delegate d)
        {
            this.hash = hash;
            this.del = d;
        }
    }
}
