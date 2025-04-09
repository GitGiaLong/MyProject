using Libraries.Custom.Structs;

namespace Libraries.Custom.Controls.Datetimes.TimeBar
{
    public class DateTimeRangeComparer : IComparer<DateTimeRange>
    {
        public int Compare(DateTimeRange x, DateTimeRange y)
        {
            if (x.Start > y.Start && x.End > y.End) return 1;
            if (x.Start < y.Start && x.End < y.End) return -1;
            return 0;
        }
    }
}
