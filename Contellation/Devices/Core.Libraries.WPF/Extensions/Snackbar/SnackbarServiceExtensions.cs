namespace Core.Libraries.WPF.Extensions.Snackbar
{

    /// <summary>
    /// Extensions for the <see cref="ISnackbarService"/>.
    /// </summary>
    public static class SnackbarServiceExtensions
    {
        /// <summary>
        /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
        /// </summary>
        /// <param name="snackbarService">The <see cref="ISnackbarService"/>.</param>
        /// <param name="title">Name displayed on top of snackbar.</param>
        /// <param name="message">Message inside the snackbar.</param>
        public static void Show(this ISnackbarService snackbarService, string title, string message)
        {
            snackbarService.Show(string.Empty, null, title, message, snackbarService.DefaultTimeOut);
        }

        ///// <summary>
        ///// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
        ///// </summary>
        ///// <param name="snackbarService">The <see cref="ISnackbarService"/>.</param>
        ///// <param name="title">Name displayed on top of snackbar.</param>
        ///// <param name="message">Message inside the snackbar.</param>
        ///// <param name="appearance">Display style.</param>
        //public static void Show(this ISnackbarService snackbarService, string title, string message)
        //{
        //    snackbarService.Show(string.Empty, null, title, message, snackbarService.DefaultTimeOut);
        //}

        ///// <summary>
        ///// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
        ///// </summary>
        ///// <param name="snackbarService">The <see cref="ISnackbarService"/>.</param>
        ///// <param name="title">Name displayed on top of snackbar.</param>
        ///// <param name="message">Message inside the snackbar.</param>
        ///// <param name="icon">Additional icon on the left.</param>
        //public static void Show(this ISnackbarService snackbarService, string title, string message,
        //    string Icon
        //)
        //{
        //    snackbarService.Show(
        //        Icon,
        //        null,
        //        title,
        //        message,
        //        snackbarService.DefaultTimeOut
        //    );
        //}

        ///// <summary>
        ///// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
        ///// </summary>
        ///// <param name="snackbarService">The <see cref="ISnackbarService"/>.</param>
        ///// <param name="title">Name displayed on top of snackbar.</param>
        ///// <param name="message">Message inside the snackbar.</param>
        ///// <param name="timeout">The time after which the snackbar should disappear.</param>
        //public static void Show(
        //    this ISnackbarService snackbarService,
        //    string title,
        //    string message,
        //    TimeSpan timeout
        //)
        //{
        //    snackbarService.Show(title, message, ControlAppearance.Secondary, null, timeout);
        //}

        ///// <summary>
        ///// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
        ///// </summary>
        ///// <param name="snackbarService">The <see cref="ISnackbarService"/>.</param>
        ///// <param name="title">Name displayed on top of snackbar.</param>
        ///// <param name="message">Message inside the snackbar.</param>
        ///// <param name="appearance">Display style.</param>
        ///// <param name="timeout">The time after which the snackbar should disappear.</param>
        //public static void Show(
        //    this ISnackbarService snackbarService,
        //    string title,
        //    string message,
        //    ControlAppearance appearance,
        //    TimeSpan timeout
        //)
        //{
        //    snackbarService.Show(title, message, appearance, null, timeout);
        //}

        ///// <summary>
        ///// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
        ///// </summary>
        ///// <param name="snackbarService">The <see cref="ISnackbarService"/>.</param>
        ///// <param name="title">Name displayed on top of snackbar.</param>
        ///// <param name="message">Message inside the snackbar.</param>
        ///// <param name="icon">Additional icon on the left.</param>
        ///// <param name="timeout">The time after which the snackbar should disappear.</param>
        //public static void Show(
        //    this ISnackbarService snackbarService,
        //    string title,
        //    string message,
        //    IconElement icon,
        //    TimeSpan timeout
        //)
        //{
        //    snackbarService.Show(title, message, ControlAppearance.Secondary, icon, timeout);
        //}
    }
}
