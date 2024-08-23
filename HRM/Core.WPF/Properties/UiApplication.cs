namespace Core.WPF.Properties
{
    /// <summary> Represents a UI application. </summary>
    public class UiApplication
    {

        private Window? _mainWindow;
        /// <summary> Gets or sets the application's main window. </summary>
        public Window? MainWindow
        {
            get => _application?.MainWindow ?? _mainWindow;
            set
            {
                if (_application != null) { _application.MainWindow = value; }

                _mainWindow = value;
            }
        }

        private readonly Application? _application;
        /// <summary> Initializes a new instance of the <see cref="UiApplication"/> class. </summary>
        public UiApplication(Application application) { _application = application; }

        /// <summary> Gets a value indicating whether the application is running outside of the desktop app context. </summary>
        public bool IsApplication => _application is not null;

        private static UiApplication? _uiApplication;
        /// <summary> Gets the current application. </summary>
        public static UiApplication Current
        {
            get
            {
                _uiApplication ??= new UiApplication(Application.Current);

                return _uiApplication;
            }
        }


        private ResourceDictionary? _resources;
        /// <summary> Gets or sets the application's resources. </summary>
        public ResourceDictionary Resources
        {
            get
            {
                if (_resources is null)
                {
                    //_resources = [];
                    _resources = new ResourceDictionary();

                    try
                    {
                        Appearance.ApplicationAccentColorManager.ApplySystemAccent();
                        var themesDictionary = new Markup.ThemesDictionary();
                        var controlsDictionary = new Markup.ControlsDictionary();
                        _resources.MergedDictionaries.Add(themesDictionary);
                        _resources.MergedDictionaries.Add(controlsDictionary);
                    }
                    catch { }
                }

                return _application?.Resources ?? _resources;
            }
            set
            {
                if (_application is not null) { _application.Resources = value; }

                _resources = value;
            }
        }

        /// <summary> Gets or sets the application's main window. </summary>
        public object TryFindResource(object resourceKey) { return Resources[resourceKey]; }

        /// <summary> Turns the application's into shutdown mode. </summary>
        public void Shutdown() { _application?.Shutdown(); }
    }
}
