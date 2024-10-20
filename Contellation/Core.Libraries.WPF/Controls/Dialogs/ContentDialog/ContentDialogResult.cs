﻿namespace Core.Libraries.WPF.Controls.Dialogs.ContentDialog
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a <see cref="ContentDialog"/>.
    /// </summary>
    public enum ContentDialogResult
    {
        /// <summary>
        /// No button was tapped.
        /// </summary>
        None,

        /// <summary>
        /// The primary button was tapped by the user.
        /// </summary>
        Primary,

        /// <summary>
        /// The secondary button was tapped by the user.
        /// </summary>
        Secondary
    }
}
