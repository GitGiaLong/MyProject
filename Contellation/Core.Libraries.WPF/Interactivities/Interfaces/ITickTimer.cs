﻿namespace Core.Libraries.WPF.Interactivities
{
    interface ITickTimer
    {
        event EventHandler Tick;
        void Start();
        void Stop();
        TimeSpan Interval { get; set; }
    }
}
