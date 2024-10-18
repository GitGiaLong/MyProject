using Core.Libraries.WPF.Structs;

namespace Core.Libraries.WPF.Controls.Datetimes.TimeBar
{
    public class ComparerGenerator
    {
        private static readonly Dictionary<Type, ComparerTypeCode> TypeCodeDic = new()
        {
            [typeof(DateTimeRange)] = ComparerTypeCode.DateTimeRange,
        };

        public static IComparer<T> GetComparer<T>()
        {
            if (TypeCodeDic.TryGetValue(typeof(T), out var comparerType))
            {
                if (comparerType == ComparerTypeCode.DateTimeRange)
                {
                    return (IComparer<T>)new DateTimeRangeComparer();
                }

                return null;
            }

            return null;
        }

        private enum ComparerTypeCode
        {
            DateTimeRange
        }
    }
}
