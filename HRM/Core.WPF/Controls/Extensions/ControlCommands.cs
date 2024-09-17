using System.Windows.Input;

namespace Core.WPF.Controls.Extensions
{
    /// <summary>
    /// All commands used by the control library (for the sake of unification, the commands that come with WPF are not used)
    /// </summary>
    public static class ControlCommands
    {
        /// <summary> Search </summary>
        public static RoutedCommand Search { get; } = new(nameof(Search), typeof(ControlCommands));

        /// <summary> Clear </summary>
        public static RoutedCommand Clear { get; } = new(nameof(Clear), typeof(ControlCommands));

        /// <summary> Switch </summary>
        public static RoutedCommand Switch { get; } = new(nameof(Switch), typeof(ControlCommands));

        /// <summary> Turn right </summary>
        public static RoutedCommand RotateRight { get; } = new(nameof(RotateRight), typeof(ControlCommands));

        /// <summary> Turn left </summary>
        public static RoutedCommand RotateLeft { get; } = new(nameof(RotateLeft), typeof(ControlCommands));

        /// <summary> Reduce(Down) </summary>
        public static RoutedCommand Reduce { get; } = new(nameof(Reduce), typeof(ControlCommands));

        /// <summary> Enlarge(Up) </summary>
        public static RoutedCommand Enlarge { get; } = new(nameof(Enlarge), typeof(ControlCommands));

        /// <summary> Restore </summary>
        public static RoutedCommand Restore { get; } = new(nameof(Restore), typeof(ControlCommands));

        /// <summary> Open </summary>
        public static RoutedCommand Open { get; } = new(nameof(Open), typeof(ControlCommands));

        /// <summary> Save </summary>
        public static RoutedCommand Save { get; } = new(nameof(Save), typeof(ControlCommands));

        /// <summary> selected </summary>
        public static RoutedCommand Selected { get; } = new(nameof(Selected), typeof(ControlCommands));

        /// <summary> Close </summary>
        public static RoutedCommand Close { get; } = new(nameof(Close), typeof(ControlCommands));

        /// <summary> Cancel </summary>
        public static RoutedCommand Cancel { get; } = new(nameof(Cancel), typeof(ControlCommands));

        /// <summary> Confirm </summary>
        public static RoutedCommand Confirm { get; } = new(nameof(Confirm), typeof(ControlCommands));

        /// <summary> Yes </summary>
        public static RoutedCommand Yes { get; } = new(nameof(Yes), typeof(ControlCommands));

        /// <summary> No </summary>
        public static RoutedCommand No { get; } = new(nameof(No), typeof(ControlCommands));

        /// <summary> Close all </summary>
        public static RoutedCommand CloseAll { get; } = new(nameof(CloseAll), typeof(ControlCommands));

        /// <summary> Close other </summary>
        public static RoutedCommand CloseOther { get; } = new(nameof(CloseOther), typeof(ControlCommands));

        /// <summary> Previous </summary>
        public static RoutedCommand Prev { get; } = new(nameof(Prev), typeof(ControlCommands));

        /// <summary> Next </summary>
        public static RoutedCommand Next { get; } = new(nameof(Next), typeof(ControlCommands));

        /// <summary> Jump </summary>
        public static RoutedCommand Jump { get; } = new(nameof(Jump), typeof(ControlCommands));

        /// <summary> Am </summary>
        public static RoutedCommand Am { get; } = new(nameof(Am), typeof(ControlCommands));

        /// <summary> Pm </summary>
        public static RoutedCommand Pm { get; } = new(nameof(Pm), typeof(ControlCommands));

        /// <summary> Sure </summary>
        public static RoutedCommand Sure { get; } = new(nameof(Sure), typeof(ControlCommands));

        /// <summary> Hour change </summary>
        public static RoutedCommand HourChange { get; } = new(nameof(HourChange), typeof(ControlCommands));

        /// <summary> Minutes change </summary>
        public static RoutedCommand MinuteChange { get; } = new(nameof(MinuteChange), typeof(ControlCommands));

        /// <summary> Seconds to change </summary>
        public static RoutedCommand SecondChange { get; } = new(nameof(SecondChange), typeof(ControlCommands));

        /// <summary> Mouse movement </summary>
        public static RoutedCommand MouseMove { get; } = new(nameof(MouseMove), typeof(ControlCommands));

        /// <summary> Open link </summary>
        public static OpenLinkCommand OpenLink { get; } = new();

        /// <summary> Close program </summary>
        public static ShutdownAppCommand ShutdownApp { get; } = new();

        ///// <summary> Front Main Window </summary>
        //public static PushMainWindow2TopCommand PushMainWindow2Top { get; } = new();

        /// <summary> Close window </summary>
        public static CloseWindowCommand CloseWindow { get; } = new();

        /// <summary> Start taking screenshots </summary>
        public static StartScreenshotCommand StartScreenshot { get; } = new();

        /// <summary> Sort by category </summary>
        public static RoutedCommand SortByCategory { get; } = new(nameof(SortByCategory), typeof(ControlCommands));

        /// <summary> Sort by name </summary>
        public static RoutedCommand SortByName { get; } = new(nameof(SortByName), typeof(ControlCommands));

        /// <summary> More </summary>
        public static RoutedCommand More { get; } = new(nameof(More), typeof(ControlCommands));

        public static RoutedCommand Toggle { get; } = new(nameof(Toggle), typeof(ControlCommands));
    }
}
