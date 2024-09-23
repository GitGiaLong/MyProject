using Core.Entities.Systems.Structs.Points;
using System.Runtime.InteropServices;

namespace Core.Entities.Systems
{

    [StructLayout(LayoutKind.Sequential)]
    public class MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }
}
