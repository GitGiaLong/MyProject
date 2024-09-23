namespace Core.Libraries.WPF.Controls.Loadings.Indicators
{
    internal class VisualStateNames : MarkupExtension
    {
        private static VisualStateNames? _activeState;
        public static VisualStateNames ActiveState => _activeState ?? (_activeState = new VisualStateNames("Active"));

        private static VisualStateNames? _inactiveState;
        public static VisualStateNames InactiveState => _inactiveState ?? (_inactiveState = new VisualStateNames("Inactive"));

        public string Name { get; }
        private VisualStateNames(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }

            Name = name;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) { return Name; }
    }
}
