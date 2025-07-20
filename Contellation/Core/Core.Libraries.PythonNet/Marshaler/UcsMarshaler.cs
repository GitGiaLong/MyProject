using Core.Libraries.PythonNet.Utils;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Marshaler
{
    /// <summary>
    /// Custom Marshaler to deal with Managed String to Native
    /// conversion differences on UCS2/UCS4.
    /// </summary>
    internal class UcsMarshaler : MarshalerBase
    {
        internal static readonly int _UCS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 2 : 4;
        internal static readonly Encoding PyEncoding = _UCS == 2 ? Encodings.UTF16 : Encodings.UTF32;
        private static readonly MarshalerBase Instance = new UcsMarshaler();

        public override IntPtr MarshalManagedToNative(object managedObj)
        {
            if (managedObj is not string s) { return IntPtr.Zero; }

            byte[] bStr = PyEncoding.GetBytes(s + "\0");
            IntPtr mem = Marshal.AllocHGlobal(bStr.Length);
            try
            {
                Marshal.Copy(bStr, 0, mem, bStr.Length);
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(mem);
                throw;
            }

            return mem;
        }

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return Instance;
        }

        public static string? PtrToStringUni(IntPtr p)
        {
            if (p == IntPtr.Zero) { return null; }

            int size = GetUnicodeByteLength(p);
            var buffer = new byte[size];
            Marshal.Copy(p, buffer, 0, size);
            return PyEncoding.GetString(buffer, 0, size);
        }

        public static int GetUnicodeByteLength(IntPtr p)
        {
            var len = 0;
            while (true)
            {
                int c = _UCS == 2 ? Marshal.ReadInt16(p, len * 2) : Marshal.ReadInt32(p, len * 4);

                if (c == 0)
                {
                    return len * _UCS;
                }
                checked
                {
                    ++len;
                }
            }
        }

        /// <summary>
        /// Utility function for Marshaling Unicode on PY3 and AnsiStr on PY2.
        /// Use on functions whose Input signatures changed between PY2/PY3.
        /// Ex. Py_SetPythonHome
        /// </summary>
        /// <param name="s">Managed String</param>
        /// <returns>
        /// Ptr to Native String ANSI(PY2)/Unicode(PY3/UCS2)/UTF32(PY3/UCS4.
        /// </returns>
        /// <remarks>
        /// You MUST deallocate the IntPtr of the Return when done with it.
        /// </remarks>
        public static IntPtr Py3UnicodePy2StringtoPtr(string s)
        {
            return Instance.MarshalManagedToNative(s);
        }

        /// <summary>
        /// Utility function for Marshaling Unicode IntPtr on PY3 and
        /// AnsiStr IntPtr on PY2 to Managed Strings. Use on Python functions
        /// whose return type changed between PY2/PY3.
        /// Ex. Py_GetPythonHome
        /// </summary>
        /// <param name="p">Native Ansi/Unicode/UTF32 String</param>
        /// <returns>
        /// Managed String
        /// </returns>
        public static string? PtrToPy3UnicodePy2String(IntPtr p)
        {
            return PtrToStringUni(p);
        }
    }
}
