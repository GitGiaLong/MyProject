namespace Core.Libraries.Interfaces.PythonNet.Platform
{
    interface ILibraryLoader
    {
        IntPtr Load(string? dllToLoad);

        IntPtr GetFunction(IntPtr hModule, string procedureName);

        void Free(IntPtr hModule);
    }
}
