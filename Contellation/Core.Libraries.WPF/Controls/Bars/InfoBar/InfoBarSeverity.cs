﻿namespace Core.Libraries.WPF.Controls.Bars.InfoBar
{
    public enum InfoBarSeverity
    {
        /// <summary>
        /// Communicates that the InfoBar is displaying general information that requires the user's attention.
        /// </summary>
        Informational = 0,

        /// <summary>
        /// Communicates that the InfoBar is displaying information regarding a long-running and/or background task
        /// that has completed successfully.
        /// </summary>
        Success = 1,

        /// <summary>
        /// Communicates that the InfoBar is displaying information regarding a condition that might cause a problem in
        /// the future.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Communicates that the InfoBar is displaying information regarding an error or problem that has occurred.
        /// </summary>
        Error = 3
    }
}
