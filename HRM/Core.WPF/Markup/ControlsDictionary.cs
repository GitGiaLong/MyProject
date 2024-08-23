namespace Core.WPF.Markup
{
    /// <summary>
    /// Provides a dictionary implementation that contains <c>Core.WPF</c> controls resources 
    /// used by components and other elements of a WPF application.
    /// </summary>
    [Localizability(LocalizationCategory.Ignore)]
    [Ambient]
    [UsableDuringInitialization(true)]
    public class ControlsDictionary : ResourceDictionary
    {
        private const string DictionaryUri = "pack://application:,,,/Core.WPF;component/Themes/Generic.xaml";

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlsDictionary"/> class.
        /// Default constructor defining <see cref="ResourceDictionary.Source"/> of the <c>Core.WPF</c> controls dictionary.
        /// </summary>
        public ControlsDictionary() { Source = new Uri(DictionaryUri, UriKind.Absolute); }
    }
}
