namespace Core.Libraries.Enums.PythonNet
{
    [Flags]
    internal enum MaybeMethodFlags
    {
        Default = 0,
        Constructor = 1,
        Static = 2,

        // TODO: other kinds of visibility
        Public = 32,
        Visibility = Public,
    }
}
