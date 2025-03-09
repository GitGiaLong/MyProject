using System.Runtime.InteropServices;

namespace Core.Entities.Systems.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BitMapInfo
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        // ReSharper disable IdentifierTypo
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        // ReSharper restore IdentifierTypo
        public int biClrUsed;
        public int biClrImportant;
        public byte bmiColors_rgbBlue;
        public byte bmiColors_rgbGreen;
        public byte bmiColors_rgbRed;
        public byte bmiColors_rgbReserved;
    }
}
