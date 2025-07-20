using Core.Libraries.PythonNet.PythonTypes;
using Core.Libraries.Structs.PythonNet.References;

namespace Core.Libraries.PythonNet.Utils
{
    /// <summary>
    /// An utility class, that can only have one value: <c>null</c>.
    /// <para>Useful for overloading operators on structs,
    /// that have meaningful concept of <c>null</c> value (e.g. pointers and references).</para>
    /// </summary>
    class NullOnly : PyObject
    {
        private NullOnly() : base(BorrowedReference.Null) { }
    }
}
