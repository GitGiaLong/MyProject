namespace Core.Libraries.WPF.Helpers
{

    /// <summary>
    /// Resource help class
    /// </summary>
    public class ResourceHelper
    {
        private static ResourceDictionary _theme;

        internal static T GetResourceInternal<T>(string key)
        {
            if (GetTheme()[key] is T resource)
            {
                return resource;
            }

            return default;
        }


        /// <summary>
        ///     get HandyControl theme
        /// </summary>
        public static ResourceDictionary GetTheme() => _theme ??= GetStandaloneTheme();

        public static ResourceDictionary GetStandaloneTheme()
        {
            return new()
            {
                Source = new Uri("pack://application:,,,/Core.Libraries.WPF;component/Controls/Windows/Window.xaml")
            };
        }
    }
}
