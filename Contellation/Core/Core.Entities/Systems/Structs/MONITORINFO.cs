using Core.Entities.Systems.Structs.Rects;

namespace Core.Entities.Systems.Structs
{
    public struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }
}
