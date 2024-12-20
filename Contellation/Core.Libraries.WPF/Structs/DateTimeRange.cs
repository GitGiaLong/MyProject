﻿using Core.Libraries.WPF.Interfaces;

namespace Core.Libraries.WPF.Structs
{
    public struct DateTimeRange : IValueRange<DateTime>
    {
        public DateTimeRange(DateTime start)
        {
            Start = start;
            End = start;
        }

        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double TotalMilliseconds => (End - Start).TotalMilliseconds;
    }
}
