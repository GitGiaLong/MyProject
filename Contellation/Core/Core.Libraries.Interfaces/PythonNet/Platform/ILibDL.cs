namespace Core.Libraries.Interfaces.PythonNet.Platform
{
    interface ILibDL
    {
        IntPtr dlopen(string? fileName, int flags);
        IntPtr dlsym(IntPtr handle, string symbol);
        int dlclose(IntPtr handle);
        IntPtr dlerror();

        int RTLD_NOW { get; }
        int RTLD_GLOBAL { get; }
        IntPtr RTLD_DEFAULT { get; }
    }
}
