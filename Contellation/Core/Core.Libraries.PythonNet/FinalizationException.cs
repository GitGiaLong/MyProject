using Core.Libraries.PythonNet.PythonTypes;
using Core.Libraries.Structs.PythonNet.References;

namespace Core.Libraries.PythonNet
{
    public class FinalizationException : Exception
    {
        public IntPtr Handle { get; }

        /// <summary>
        /// Gets the object, whose finalization failed.
        ///
        /// <para>If this function crashes, you can also try <see cref="DebugGetObject"/>,
        /// which does not attempt to increase the object reference count.</para>
        /// </summary>
        public PyObject GetObject() => new(new BorrowedReference(this.Handle));
        /// <summary>
        /// Gets the object, whose finalization failed without incrementing
        /// its reference count. This should only ever be called during debugging.
        /// When the result is disposed or finalized, the program will crash.
        /// </summary>
        public PyObject DebugGetObject()
        {
            IntPtr dangerousNoIncRefCopy = this.Handle;
            return new(StolenReference.Take(ref dangerousNoIncRefCopy));
        }

        public FinalizationException(string message, IntPtr disposable, Exception innerException)
            : base(message, innerException)
        {
            if (disposable == IntPtr.Zero) throw new ArgumentNullException(nameof(disposable));
            this.Handle = disposable;
        }

        protected FinalizationException(string message, IntPtr disposable)
            : base(message)
        {
            if (disposable == IntPtr.Zero) throw new ArgumentNullException(nameof(disposable));
            this.Handle = disposable;
        }
    }
}
