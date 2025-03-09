using Libraries.Custom.Structs.Rects;

namespace Libraries.Custom.Structs
{
    public struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }
}
