using System.Runtime.InteropServices;

namespace Core.Libraries.Structs.PythonNet.Py
{
    [StructLayout(LayoutKind.Sequential)]
    struct PyMethodDef
    {
        public IntPtr ml_name;
        public IntPtr ml_meth;
        public int ml_flags;
        public IntPtr ml_doc;
    }
}
