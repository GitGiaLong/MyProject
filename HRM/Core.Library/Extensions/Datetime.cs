using System.Globalization;

namespace Core.Library.Extensions
{
    public static class Datetime
    {
        /// <summary>
        /// NullDate của Mongo
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNullDate()
        {
            return ConvertFromStringToDateTime("01/01/1900");
        }

        /// <summary>
        /// Kiểm tra ngày Datetime?
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsNullDate(this DateTime dateTime)
        {
            return dateTime.CompareTo(new DateTime(1, 1, 1)) == 0;
        }

        public static DateTime ConvertFromStringToDateTime(string p_strDateTime)
        {
            try
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                return DateTime.Parse(p_strDateTime, culture);
            }
            catch (Exception)
            {
                return ConvertFromStringToDateTime(p_strDateTime, "dd/MM/yyyy");
            }
        }

        /// <summary>
        /// convert string to datetime theo định dạng do mình qui định. 
        /// VD p_strDate = 14/03/2017 23:12:00 thì p_strFormat tương ứng: dd/MM/yyyy HH:mm:ss
        /// </summary>
        public static DateTime ConvertFromStringToDateTime(string p_strDate, string p_strFormat)
        {
            if (p_strDate == "")
                return GetNullDate();

            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(p_strDate, p_strFormat, provider);
        }

        /// <summary>
        /// Convert thành đầu ngày
        /// </summary>
        /// <param name="p_dtmDate"></param>
        /// <returns></returns>
        public static DateTime GetBeginningOfTheDay(DateTime? p_dtmDate)
        {
            DateTime v_dtmRes = ConvertFromStringToDateTime(p_dtmDate?.ToString("MM/dd/yyyy") + " 00:00:00");
            return v_dtmRes;
        }

        /// <summary>
        /// Convert thành cuối ngày
        /// </summary>
        /// <param name="p_dtmDate"></param>
        /// <returns></returns>
        public static DateTime GetEndingOfTheDay(DateTime? p_dtmDate)
        {
            DateTime v_dtmRes = ConvertFromStringToDateTime(p_dtmDate?.ToString("MM/dd/yyyy") + " 23:59:59");
            return v_dtmRes;
        }

        public static DateTime AddDayIncludeSaturday(DateTime p_dtmNow, int p_iDay)
        {
            int v_iCount = 0;
            int v_iSub = 1;
            if (p_iDay < 0)
                v_iSub = -1;
            DateTime v_dtRes = p_dtmNow;

            while (v_iCount < Math.Abs(p_iDay))
            {
                v_iCount++;
                v_dtRes = v_dtRes.AddDays(v_iSub);

                while (v_dtRes.DayOfWeek == DayOfWeek.Sunday)
                    v_dtRes = v_dtRes.AddDays(v_iSub);
            }

            return v_dtRes;
        }

        /// <summary>
        /// Lấy ngày đầu tuần
        /// </summary>
        /// <param name="p_dtmData"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime p_dtmData)
        {
            DateTime v_dtmRes = p_dtmData;

            while (v_dtmRes.DayOfWeek != DayOfWeek.Monday)
                v_dtmRes = v_dtmRes.AddDays(-1);

            return v_dtmRes;
        }

        /// <summary>
        /// Lấy ngày đầu tháng
        /// </summary>
        /// <param name="p_dtmData"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(DateTime p_dtmData)
        {
            DateTime v_dtmRes = p_dtmData;

            while (v_dtmRes.Day != 1)
                v_dtmRes = v_dtmRes.AddDays(-1);

            return v_dtmRes;
        }

        public static DateTime GetFirstDayOfMonth(int? month = null)
        {
            DateTime now = DateTime.Now;
            return month is null ? new DateTime(now.Year, now.Month, 1) : new DateTime(now.Year, (int)month, 1);
        }

        public static DateTime GetLastDateOfMonth(int? month = null)
        {
            DateTime startDate = GetFirstDayOfMonth(month);
            return startDate.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Lấy ngày đầu nam
        /// </summary>
        /// <param name="p_dtmData"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfYear(DateTime p_dtmData)
        {
            DateTime v_dtmRes = p_dtmData;

            while (v_dtmRes.DayOfYear != 1)
                v_dtmRes = v_dtmRes.AddDays(-1);

            return v_dtmRes;
        }

        /// <summary>
        /// Kiểm tra 1 ngày có nằm giữa 2 ngày nào đó không ?
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rangeBeg"></param>
        /// <param name="rangeEnd"></param>
        /// <returns></returns>
        public static bool Between(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        /// <summary>
        /// Tính tuổi của 1 người
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age))
                age--;
            return age;
        }

        /// <summary>
        /// Trả về danh sách các ngày trong 1 khoảng thời gian
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException($"endDate must be greater than or equal to startDate");
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }
    }
}
