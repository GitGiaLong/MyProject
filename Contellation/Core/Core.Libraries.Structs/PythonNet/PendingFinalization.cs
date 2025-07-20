using Core.Libraries.PythonNet.Runtimes;
using Core.Libraries.PythonNet.Types;
using Core.Libraries.Structs.PythonNet.References;

namespace Core.Libraries.Structs.PythonNet
{
    struct PendingFinalization
    {
        public IntPtr PyObj;
        public BorrowedReference Ref => new(PyObj);
        public ManagedType? Managed => ManagedType.GetManagedObject(Ref);
        public nint RefCount => Runtime.Refcount(Ref);
        public int RuntimeRun;
#if TRACE_ALLOC
        public string StackTrace;
#endif
    }
}
