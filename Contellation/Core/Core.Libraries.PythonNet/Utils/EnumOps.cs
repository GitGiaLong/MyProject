using Core.Libraries.PythonNet.Attributes;
using Core.Libraries.PythonNet.PythonTypes;

namespace Core.Libraries.PythonNet.Utils
{
    [Ops]
    internal static class EnumOps<T> where T : Enum
    {
        [ForbidPythonThreads]
#pragma warning disable IDE1006 // Naming Styles - must match Python
        public static PyInt __index__(T value)
#pragma warning restore IDE1006 // Naming Styles
            => typeof(T).GetEnumUnderlyingType() == typeof(UInt64)
            ? new PyInt(Convert.ToUInt64(value))
            : new PyInt(Convert.ToInt64(value));
        [ForbidPythonThreads]
#pragma warning disable IDE1006 // Naming Styles - must match Python
        public static PyInt __int__(T value) => __index__(value);
#pragma warning restore IDE1006 // Naming Styles
    }
}
