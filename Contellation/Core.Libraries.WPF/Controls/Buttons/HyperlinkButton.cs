using System.Diagnostics;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Button that opens a URL in a web browser.
    /// </summary>
    public class HyperlinkButton : Button
    {
        /// <summary> Gets or sets the URL (or application shortcut) to open. </summary>
        public string NavigateUri
        {
            get { return GetValue(NavigateUriProperty) as string ?? string.Empty; }
            set { SetValue(NavigateUriProperty, value); }
        }
        /// <summary>Identifies the <see cref="NavigateUri"/> dependency property.</summary>
        public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(nameof(NavigateUri), typeof(string),
            typeof(HyperlinkButton), new PropertyMetadata(string.Empty));

        protected override void OnClick()
        {
            base.OnClick();
            if (string.IsNullOrEmpty(NavigateUri)) { return; }

            try
            {
                Debug.WriteLine($"INFO | HyperlinkButton clicked, with href: {NavigateUri}", "Core.Libraries.WPF.HyperlinkButton");

                ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri) { UseShellExecute = true };

                _ = Process.Start(sInfo);
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }
    }
}
