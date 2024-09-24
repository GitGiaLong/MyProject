using Core.Libraries.WPF.Controls.Snackbars;

namespace Core.Libraries.WPF.Extensions.Snackbar
{
    /// <summary>
    /// A service that provides methods related to displaying the <see cref="Snackbar"/>.
    /// </summary>
    public class SnackbarService : ISnackbarService
    {
        private SnackbarPresenter? _presenter;

        private Controls.Snackbar? _snackbar;

        /// <inheritdoc />
        public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(5);

        /// <inheritdoc />
        public void SetSnackbarPresenter(SnackbarPresenter contentPresenter) { _presenter = contentPresenter; }

        /// <inheritdoc />
        public SnackbarPresenter? GetSnackbarPresenter() { return _presenter; }

        /// <inheritdoc />
        public void Show(string Icon, FontFamily FontFamilyIcon, string title, string message, TimeSpan timeout)
        {
            if (_presenter is null)
            {
                throw new InvalidOperationException($"The SnackbarPresenter was never set");
            }

            _snackbar ??= new Controls.Snackbar(_presenter);

            _snackbar.SetCurrentValue(Controls.Snackbar.TitleProperty, title);
            _snackbar.SetCurrentValue(System.Windows.Controls.ContentControl.ContentProperty, message);
            //_snackbar.SetCurrentValue(Snackbar.AppearanceProperty, appearance);
            //_snackbar.SetCurrentValue(Snackbar.IconProperty, icon);
            _snackbar.SetCurrentValue(Controls.Snackbar.TimeoutProperty, timeout.TotalSeconds == 0 ? DefaultTimeOut : timeout );

            _snackbar.Show(true);
        }
    }
}
