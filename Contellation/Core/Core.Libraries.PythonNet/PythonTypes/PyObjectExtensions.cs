using Core.Libraries.PythonNet.References;
using Core.Libraries.Structs.PythonNet.References;

namespace Core.Libraries.PythonNet.PythonTypes
{
    internal static class PyObjectExtensions
    {
        internal static NewReference NewReferenceOrNull(this PyObject? self)
            => self is null || self.IsDisposed ? default : new NewReference(self);

        internal static BorrowedReference BorrowNullable(this PyObject? self)
            => self is null ? default : self.Reference;
    }
}
