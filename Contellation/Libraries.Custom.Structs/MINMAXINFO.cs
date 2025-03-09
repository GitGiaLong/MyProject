using Libraries.Custom.Structs.Points;

namespace Libraries.Custom.Structs
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
