﻿using Core.Entities.Systems.Structs.Rects;
using System.Runtime.InteropServices;

namespace Core.Entities.Systems.Structs
{

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }
}
