namespace Core.Libraries.Enums.PythonNet
{
    /// <summary>
    /// TypeFlags(): The actual bit values for the Type Flags stored
    /// in a class.
    /// Note that the two values reserved for stackless have been put
    /// to good use as PythonNet specific flags (Managed and Subclass)
    /// </summary>
    // Py_TPFLAGS_*
    [Flags]
    public enum TypeFlags : long
    {
        HeapType = (1 << 9),
        BaseType = (1 << 10),
        Ready = (1 << 12),
        Readying = (1 << 13),
        HaveGC = (1 << 14),
        // 15 and 16 are reserved for stackless
        HaveStacklessExtension = 0,
        /* XXX Reusing reserved constants */
        /// <remarks>PythonNet specific</remarks>
        HasClrInstance = (1 << 15),
        /// <remarks>PythonNet specific</remarks>
        Subclass = (1 << 16),
        /* Objects support nb_index in PyNumberMethods */
        HaveVersionTag = (1 << 18),
        ValidVersionTag = (1 << 19),
        IsAbstract = (1 << 20),
        HaveNewBuffer = (1 << 21),
        // TODO: Implement FastSubclass functions
        IntSubclass = (1 << 23),
        LongSubclass = (1 << 24),
        ListSubclass = (1 << 25),
        TupleSubclass = (1 << 26),
        StringSubclass = (1 << 27),
        UnicodeSubclass = (1 << 28),
        DictSubclass = (1 << 29),
        BaseExceptionSubclass = (1 << 30),
        TypeSubclass = (1 << 31),

        Default = (HaveStacklessExtension | HaveVersionTag),
    }
}
