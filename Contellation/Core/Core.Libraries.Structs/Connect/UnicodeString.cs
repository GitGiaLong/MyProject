using System.Runtime.InteropServices;

namespace Core.Libraries.Structs.Connect
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/aa380518.aspx
    // https://msdn.microsoft.com/en-us/library/windows/hardware/ff564879.aspx
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct UnicodeString
    {
        /// <summary>
        /// Length, in bytes, not including the null, if any.
        /// </summary>
        internal ushort Length;

        /// <summary>
        /// Max size of the buffer in bytes
        /// </summary>
        internal ushort MaximumLength;

        /// <summary>
        /// Pointer to the buffer used to contain the wide characters of the string.
        /// </summary>
        internal char* Buffer;

        public UnicodeString(char* buffer, int length)
        {
            Length = checked((ushort)(length * sizeof(char)));
            MaximumLength = checked((ushort)(length * sizeof(char)));
            Buffer = buffer;
        }
    }
}
