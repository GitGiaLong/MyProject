namespace Core.WPF.Controls.Loadings.Indicators
{
    internal class VisualStateGroupNames : MarkupExtension
    {
        private static VisualStateGroupNames? _internalActiveStates;
        public static VisualStateGroupNames ActiveStates => _internalActiveStates ?? (_internalActiveStates = new VisualStateGroupNames("ActiveStates"));

        private static VisualStateGroupNames? _sizeStates;
        public static VisualStateGroupNames SizeStates => _sizeStates ?? (_sizeStates = new VisualStateGroupNames("SizeStates"));

        public string Name { get; }
        private VisualStateGroupNames(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }

            Name = name;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) { return Name; }
    }
}
