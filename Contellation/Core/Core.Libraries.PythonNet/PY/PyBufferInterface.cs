using Core.Libraries.Enums.PythonNet;
using Core.Libraries.Structs.PythonNet.Py;
using Core.Libraries.Structs.PythonNet.References;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.PY
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int GetBufferProc(BorrowedReference obj, out Py_buffer buffer, PyBUF flags);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ReleaseBufferProc(BorrowedReference obj, ref Py_buffer buffer);
}
