using Core.Libraries.WPF.Extensions.CommandControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.Libraries.WPF.Extensions
{

    /// <summary>
    /// Tất cả các lệnh được thư viện điều khiển sử dụng (để thống nhất, các lệnh đi kèm với WPF sẽ không được sử dụng)
    /// </summary>
    public class CommandControl
    {
        /// <summary>
        /// Search
        /// </summary>
        public static RoutedCommand Search { get; } = new(nameof(Search), typeof(CommandControl));

        /// <summary>
        /// Clear
        /// </summary>
        public static RoutedCommand Clear { get; } = new(nameof(Clear), typeof(CommandControl));

        /// <summary>
        /// Switch
        /// </summary>
        public static RoutedCommand Switch { get; } = new(nameof(Switch), typeof(CommandControl));

        /// <summary>
        /// RotateRight
        /// </summary>
        public static RoutedCommand RotateRight { get; } = new(nameof(RotateRight), typeof(CommandControl));

        /// <summary>
        /// RotateLeft
        /// </summary>
        public static RoutedCommand RotateLeft { get; } = new(nameof(RotateLeft), typeof(CommandControl));

        /// <summary>
        /// Reduce
        /// </summary>
        public static RoutedCommand Reduce { get; } = new(nameof(Reduce), typeof(CommandControl));

        /// <summary>
        /// Enlarge
        /// </summary>
        public static RoutedCommand Enlarge { get; } = new(nameof(Enlarge), typeof(CommandControl));

        /// <summary>
        /// Restore
        /// </summary>
        public static RoutedCommand Restore { get; } = new(nameof(Restore), typeof(CommandControl));

        /// <summary>
        /// Open
        /// </summary>
        public static RoutedCommand Open { get; } = new(nameof(Open), typeof(CommandControl));

        /// <summary>
        /// Save
        /// </summary>
        public static RoutedCommand Save { get; } = new(nameof(Save), typeof(CommandControl));

        /// <summary>
        /// Selected
        /// </summary>
        public static RoutedCommand Selected { get; } = new(nameof(Selected), typeof(CommandControl));

        /// <summary>
        /// Close
        /// </summary>
        public static RoutedCommand Close { get; } = new(nameof(Close), typeof(CommandControl));

        /// <summary>
        /// Cancel
        /// </summary>
        public static RoutedCommand Cancel { get; } = new(nameof(Cancel), typeof(CommandControl));

        /// <summary>
        /// Confirm
        /// </summary>
        public static RoutedCommand Confirm { get; } = new(nameof(Confirm), typeof(CommandControl));

        /// <summary>
        /// Yes
        /// </summary>
        public static RoutedCommand Yes { get; } = new(nameof(Yes), typeof(CommandControl));

        /// <summary>
        /// No
        /// </summary>
        public static RoutedCommand No { get; } = new(nameof(No), typeof(CommandControl));

        /// <summary>
        /// CloseAll
        /// </summary>
        public static RoutedCommand CloseAll { get; } = new(nameof(CloseAll), typeof(CommandControl));

        /// <summary>
        /// CloseOther
        /// </summary>
        public static RoutedCommand CloseOther { get; } = new(nameof(CloseOther), typeof(CommandControl));

        /// <summary>
        /// Prev
        /// </summary>
        public static RoutedCommand Prev { get; } = new(nameof(Prev), typeof(CommandControl));

        /// <summary>
        /// Next
        /// </summary>
        public static RoutedCommand Next { get; } = new(nameof(Next), typeof(CommandControl));

        /// <summary>
        /// Jump
        /// </summary>
        public static RoutedCommand Jump { get; } = new(nameof(Jump), typeof(CommandControl));

        /// <summary>
        /// Am
        /// </summary>
        public static RoutedCommand Am { get; } = new(nameof(Am), typeof(CommandControl));

        /// <summary>
        /// Pm
        /// </summary>
        public static RoutedCommand Pm { get; } = new(nameof(Pm), typeof(CommandControl));

        /// <summary>
        /// Sure
        /// </summary>
        public static RoutedCommand Sure { get; } = new(nameof(Sure), typeof(CommandControl));

        /// <summary>
        /// HourChange
        /// </summary>
        public static RoutedCommand HourChange { get; } = new(nameof(HourChange), typeof(CommandControl));

        /// <summary>
        /// MinuteChange
        /// </summary>
        public static RoutedCommand MinuteChange { get; } = new(nameof(MinuteChange), typeof(CommandControl));

        /// <summary>
        /// SecondChange
        /// </summary>
        public static RoutedCommand SecondChange { get; } = new(nameof(SecondChange), typeof(CommandControl));

        /// <summary>
        ///     鼠标移动
        /// </summary>
        public static RoutedCommand MouseMove { get; } = new(nameof(MouseMove), typeof(CommandControl));

        /// <summary>
        /// OpenLink
        /// </summary>
        //public static OpenLinkCommand OpenLink { get; } = new();

        /// <summary>
        /// Close program
        /// </summary>
        public static ShutdownAppCommand ShutdownApp { get; } = new();

        /// <summary>
        /// close window
        /// </summary>
        public static CloseWindowCommand CloseWindow { get; } = new();

        /// <summary>
        /// Start Screenshot
        /// </summary>
        //public static StartScreenshotCommand StartScreenshot { get; } = new();

        /// <summary>
        /// SortByCategory
        /// </summary>
        public static RoutedCommand SortByCategory { get; } = new(nameof(SortByCategory), typeof(CommandControl));

        /// <summary>
        /// SortByName
        /// </summary>
        public static RoutedCommand SortByName { get; } = new(nameof(SortByName), typeof(CommandControl));

        /// <summary>
        /// More
        /// </summary>
        public static RoutedCommand More { get; } = new(nameof(More), typeof(CommandControl));

        public static RoutedCommand Toggle { get; } = new(nameof(Toggle), typeof(CommandControl));
    }
}
