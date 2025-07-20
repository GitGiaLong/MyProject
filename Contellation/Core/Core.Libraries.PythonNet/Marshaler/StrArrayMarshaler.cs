using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Marshaler
{
    /// <summary>
    /// Custom Marshaler to deal with Managed String Arrays to Native
    /// conversion differences on UCS2/UCS4.
    /// </summary>
    internal class StrArrayMarshaler : MarshalerBase
    {
        private static readonly MarshalerBase Instance = new StrArrayMarshaler();
        private static readonly Encoding PyEncoding = UcsMarshaler.PyEncoding;

        public override IntPtr MarshalManagedToNative(object managedObj)
        {
            if (managedObj is not string[] argv) { return IntPtr.Zero; }

            int totalStrLength = argv.Sum(arg => arg.Length + 1);
            int memSize = argv.Length * IntPtr.Size + totalStrLength * UcsMarshaler._UCS;

            IntPtr mem = Marshal.AllocHGlobal(memSize);
            try
            {
                // Preparing array of pointers to strings
                IntPtr curStrPtr = mem + argv.Length * IntPtr.Size;
                for (var i = 0; i < argv.Length; i++)
                {
                    byte[] bStr = PyEncoding.GetBytes(argv[i] + "\0");
                    Marshal.Copy(bStr, 0, curStrPtr, bStr.Length);
                    Marshal.WriteIntPtr(mem + i * IntPtr.Size, curStrPtr);
                    curStrPtr += bStr.Length;
                }
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(mem);
                throw;
            }

            return mem;
        }

        public static ICustomMarshaler GetInstance(string? cookie)
        {
            return Instance;
        }
    }
}
