using System.IO;

namespace Core.Libraries.WPF.Controls.Datetimes.Scheduletimelines
{
    public static class Stuff
    {
        public static double ToPixcel(this TimeSpan time, TimeSpan spanDt, double spanPx) => time.TotalMilliseconds / spanDt.TotalMilliseconds * spanPx;
        public static T GetCopy<T>(this T element) where T : UIElement
        {
            using (var ms = new MemoryStream())
            {
                XamlWriter.Save(element, ms);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)XamlReader.Load(ms);
            }
        }

    }
}
