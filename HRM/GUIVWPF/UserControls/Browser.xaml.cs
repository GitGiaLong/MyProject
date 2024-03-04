using System.Windows.Controls;
using System.Windows.Input;

namespace GUIVWPF.UserControls
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : UserControl
    {
        public Browser()
        {
            InitializeComponent();
            WebBrowser.Navigate("http://www.google.com");
        }

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                WebBrowser.Navigate(txtUrl.Text);
        }

        private void wbSample_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            txtUrl.Text = e.Uri.OriginalString;
        }
    }
}
