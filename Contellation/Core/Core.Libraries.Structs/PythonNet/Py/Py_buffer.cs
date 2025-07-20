using System.Runtime.InteropServices;

namespace Core.Libraries.Structs.PythonNet.Py
{
    /* buffer interface */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct Py_buffer
    {
        public IntPtr buf;
        public IntPtr obj;        /* owned reference */
        /// <summary>Buffer size in bytes</summary>
        [MarshalAs(UnmanagedType.SysInt)]
        public nint len;
        [MarshalAs(UnmanagedType.SysInt)]
        public nint itemsize;  /* This is Py_ssize_t so it can be
                             pointed to by strides in simple case.*/
        [MarshalAs(UnmanagedType.Bool)]
        public bool _readonly;
        public int ndim;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? format;
        public IntPtr shape;
        public IntPtr strides;
        public IntPtr suboffsets;
        public IntPtr _internal;
    }
}
