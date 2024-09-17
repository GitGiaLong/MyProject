using Core.WPF.Helpers;
using Core.WPF.Interactivities;
using System.Globalization;
using System.Windows.Data;

namespace Core.WPF.Properties
{
    public class LangProvider
    {
        internal static LangProvider Instance { get; } = ResourceHelper.GetResourceInternal<LangProvider>("Langs");

        private static string CultureInfoStr;

        internal static CultureInfo Culture
        {
            get => ExceptionStringTable.Culture;
            set
            {
                if (value == null) return;
                if (Equals(CultureInfoStr, value.EnglishName)) return;
                ExceptionStringTable.Culture = value;
                CultureInfoStr = value.EnglishName;

                //Instance.UpdateLangs();
            }
        }

        public static string GetLang(string key) => ExceptionStringTable.ResourceManager.GetString(key, Culture);

        public static void SetLang(DependencyObject dependencyObject, DependencyProperty dependencyProperty, string key) =>
            BindingOperations.SetBinding(dependencyObject, dependencyProperty, new Binding(key)
            {
                Source = Instance,
                Mode = BindingMode.OneWay
            });

    }
}
