using System.Diagnostics;

namespace Core.WPF.Interactivities.Extensions
{
    public class LaunchUriOrFileAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(LaunchUriOrFileAction));

        public LaunchUriOrFileAction() { }

        public string Path
        {
            get { return (string)this.GetValue(PathProperty); }
            set { this.SetValue(PathProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null && !string.IsNullOrEmpty(this.Path))
            {
                var processStartInfo = new ProcessStartInfo(this.Path)
                {
                    UseShellExecute = true
                };
                Process.Start(processStartInfo);
            }
        }
    }
}
