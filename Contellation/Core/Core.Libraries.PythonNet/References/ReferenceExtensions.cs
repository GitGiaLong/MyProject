using Core.Libraries.Structs.PythonNet.References;
using System.Diagnostics.Contracts;

namespace Core.Libraries.PythonNet.References
{
    static class ReferenceExtensions
    {
        /// <summary>
        /// Checks if the reference points to Python object <c>None</c>.
        /// </summary>
        [Pure]
        public static bool IsNone(this in NewReference reference) => reference.BorrowNullable() == PyNone;

        /// <summary>
        /// Checks if the reference points to Python object <c>None</c>.
        /// </summary>
        [Pure]
        public static bool IsNone(this BorrowedReference reference) => reference == PyNone;
    }
}
