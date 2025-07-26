using Core.Libraries.PythonNet.Attributes;
using Core.Libraries.PythonNet.Python;
using Core.Libraries.PythonNet.References;
using Core.Libraries.PythonNet.Runtimes;
using Core.Libraries.PythonNet.Utils;
using Core.Libraries.Structs.PythonNet;
using Core.Libraries.Structs.PythonNet.Py;
using Core.Libraries.Structs.PythonNet.References;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;

namespace Core.Libraries.PythonNet
{
    public class Finalizer
    {
        public class CollectArgs : EventArgs
        {
            public int ObjectCount { get; set; }
        }

        public class ErrorArgs : EventArgs
        {
            public ErrorArgs(Exception error)
            {
                Error = error ?? throw new ArgumentNullException(nameof(error));
            }
            public bool Handled { get; set; }
            public Exception Error { get; }
        }

        public static Finalizer Instance { get; } = new();

        public event EventHandler<CollectArgs>? BeforeCollect;
        public event EventHandler<ErrorArgs>? ErrorHandler;

        const int DefaultThreshold = 200;
        [DefaultValue(DefaultThreshold)]
        public int Threshold { get; set; } = DefaultThreshold;

        bool started;

        [DefaultValue(true)]
        public bool Enable { get; set; } = true;

        private readonly ConcurrentQueue<PendingFinalization> _objQueue = new();
        private readonly ConcurrentQueue<PendingFinalization> _derivedQueue = new();
        private readonly ConcurrentQueue<Py_buffer> _bufferQueue = new();
        private int _throttled;

        #region FINALIZER_CHECK

#if FINALIZER_CHECK
        private readonly object _queueLock = new object();
        internal bool RefCountValidationEnabled { get; set; } = true;
#else
        internal bool RefCountValidationEnabled { get; set; } = false;
#endif
        // Keep these declarations for compat even no FINALIZER_CHECK
        internal class IncorrectFinalizeArgs : EventArgs
        {
            public IncorrectFinalizeArgs(IntPtr handle, IReadOnlyCollection<IntPtr> imacted)
            {
                Handle = handle;
                ImpactedObjects = imacted;
            }
            public IntPtr Handle { get; }
            public BorrowedReference Reference => new(Handle);
            public IReadOnlyCollection<IntPtr> ImpactedObjects { get; }
        }

        internal class IncorrectRefCountException : Exception
        {
            public IntPtr PyPtr { get; internal set; }
            string? message;
            public override string Message
            {
                get
                {
                    if (message is not null) { return message; }
                    var gil = PythonEngine.AcquireLock();
                    try
                    {
                        using var pyname = PyObject_Str(new BorrowedReference(PyPtr));
                        string name = GetManagedString(pyname.BorrowOrThrow()) ?? Util.BadStr;
                        message = $"<{name}> may has a incorrect ref count";
                    }
                    finally
                    {
                        PythonEngine.ReleaseLock(gil);
                    }
                    return message;
                }
            }

            internal IncorrectRefCountException(IntPtr ptr) { PyPtr = ptr; }
        }

        internal delegate bool IncorrectRefCntHandler(object sender, IncorrectFinalizeArgs e);
#pragma warning disable 414
        internal event IncorrectRefCntHandler? IncorrectRefCntResolver = null;
#pragma warning restore 414
        internal bool ThrowIfUnhandleIncorrectRefCount { get; set; } = true;

        #endregion

        [ForbidPythonThreads]
        public void Collect() => this.DisposeAll();

        internal void ThrottledCollect()
        {
            if (!started) { throw new InvalidOperationException($"{nameof(PythonEngine)} is not initialized"); }

            _throttled = unchecked(this._throttled + 1);
            if (!started || !Enable || _throttled < Threshold) { return; }
            _throttled = 0;
            this.Collect();
        }

        internal List<IntPtr> GetCollectedObjects()
        {
            return _objQueue.Select(o => o.PyObj).ToList();
        }

        internal void AddFinalizedObject(ref IntPtr obj, int run
#if TRACE_ALLOC
                                         , StackTrace stackTrace
#endif
        )
        {
            Debug.Assert(obj != IntPtr.Zero);
            if (!Enable) { return; }

            Debug.Assert(Refcount(new BorrowedReference(obj)) > 0);

#if FINALIZER_CHECK
            lock (_queueLock)
#endif
            {
                this._objQueue.Enqueue(new PendingFinalization
                {
                    PyObj = obj,
                    RuntimeRun = run,
#if TRACE_ALLOC
                    StackTrace = stackTrace.ToString(),
#endif
                });
            }
            obj = IntPtr.Zero;
        }

        internal void AddDerivedFinalizedObject(ref IntPtr derived, int run)
        {
            if (derived == IntPtr.Zero) { throw new ArgumentNullException(nameof(derived)); }

            if (!Enable) { return; }

            var pending = new PendingFinalization { PyObj = derived, RuntimeRun = run };
            derived = IntPtr.Zero;
            _derivedQueue.Enqueue(pending);
        }

        internal void AddFinalizedBuffer(ref Py_buffer buffer)
        {
            if (buffer.obj == IntPtr.Zero) { throw new ArgumentNullException(nameof(buffer)); }

            if (!Enable) { return; }

            var pending = buffer;
            buffer = default;
            _bufferQueue.Enqueue(pending);
        }

        internal static void Initialize() { Instance.started = true; }

        internal static void Shutdown()
        {
            Instance.DisposeAll();
            Instance.started = false;
        }

        internal nint DisposeAll(bool disposeObj = true, bool disposeDerived = true, bool disposeBuffer = true)
        {
            if (_objQueue.IsEmpty && _derivedQueue.IsEmpty && _bufferQueue.IsEmpty) { return 0; }

            nint collected = 0;

            BeforeCollect?.Invoke(this, new CollectArgs()
            {
                ObjectCount = _objQueue.Count
            });
#if FINALIZER_CHECK
            lock (_queueLock)
#endif
            {
#if FINALIZER_CHECK
                ValidateRefCount();
#endif
                PyErr_Fetch(out var errType, out var errVal, out var traceback);
                Debug.Assert(errType.IsNull());

                int run = GetRun();

                try
                {
                    if (disposeObj) while (!_objQueue.IsEmpty)
                        {
                            if (!_objQueue.TryDequeue(out var obj)) { continue; }

                            if (obj.RuntimeRun != run)
                            {
                                HandleFinalizationException(obj.PyObj, new RuntimeShutdownException(obj.PyObj));
                                continue;
                            }

                            IntPtr copyForException = obj.PyObj;
                            XDecref(StolenReference.Take(ref obj.PyObj));
                            collected++;
                            try { CheckExceptionOccurred(); }
                            catch (Exception e) { HandleFinalizationException(obj.PyObj, e); }
                        }

                    if (disposeDerived) while (!_derivedQueue.IsEmpty)
                        {
                            if (!_derivedQueue.TryDequeue(out var derived)) { continue; }

                            if (derived.RuntimeRun != run)
                            {
                                HandleFinalizationException(derived.PyObj, new RuntimeShutdownException(derived.PyObj));
                                continue;
                            }

#pragma warning disable CS0618 // Type or member is obsolete. OK for internal use
                            PythonDerivedType.Finalize(derived.PyObj);
#pragma warning restore CS0618 // Type or member is obsolete

                            collected++;
                        }

                    if (disposeBuffer) while (!_bufferQueue.IsEmpty)
                        {
                            if (!_bufferQueue.TryDequeue(out var buffer)) { continue; }

                            PyBuffer_Release(ref buffer);
                            collected++;
                        }
                }
                finally
                {
                    // Python requires finalizers to preserve exception:
                    // https://docs.python.org/3/extending/newtypes.html#finalization-and-de-allocation
                    PyErr_Restore(errType.StealNullable(), errVal.StealNullable(), traceback.StealNullable());
                }
            }
            return collected;
        }

        void HandleFinalizationException(IntPtr obj, Exception cause)
        {
            var errorArgs = new ErrorArgs(cause);

            ErrorHandler?.Invoke(this, errorArgs);

            if (!errorArgs.Handled)
            {
                throw new FinalizationException("Python object finalization failed", disposable: obj, innerException: cause);
            }
        }

#if FINALIZER_CHECK
        private void ValidateRefCount()
        {
            if (!RefCountValidationEnabled)
            {
                return;
            }
            var counter = new Dictionary<IntPtr, long>();
            var holdRefs = new Dictionary<IntPtr, long>();
            var indexer = new Dictionary<IntPtr, List<IntPtr>>();
            foreach (var obj in _objQueue)
            {
                var handle = obj;
                if (!counter.ContainsKey(handle))
                {
                    counter[handle] = 0;
                }
                counter[handle]++;
                if (!holdRefs.ContainsKey(handle))
                {
                    holdRefs[handle] = Refcount(handle);
                }
                List<IntPtr> objs;
                if (!indexer.TryGetValue(handle, out objs))
                {
                    objs = new List<IntPtr>();
                    indexer.Add(handle, objs);
                }
                objs.Add(obj);
            }
            foreach (var pair in counter)
            {
                IntPtr handle = pair.Key;
                long cnt = pair.Value;
                // Tracked handle's ref count is larger than the object's holds
                // it may take an unspecified behaviour if it decref in Dispose
                if (cnt > holdRefs[handle])
                {
                    var args = new IncorrectFinalizeArgs()
                    {
                        Handle = handle,
                        ImpactedObjects = indexer[handle]
                    };
                    bool handled = false;
                    if (IncorrectRefCntResolver != null)
                    {
                        var funcList = IncorrectRefCntResolver.GetInvocationList();
                        foreach (IncorrectRefCntHandler func in funcList)
                        {
                            if (func(this, args))
                            {
                                handled = true;
                                break;
                            }
                        }
                    }
                    if (!handled && ThrowIfUnhandleIncorrectRefCount)
                    {
                        throw new IncorrectRefCountException(handle);
                    }
                }
                // Make sure no other references for PyObjects after this method
                indexer[handle].Clear();
            }
            indexer.Clear();
        }
#endif
    }
}
