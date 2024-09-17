namespace Core.WPF.Extensions
{
    public static class UIElementExtension
    {
        /// <summary>
        /// display elements
        /// </summary>
        /// <param name="element"></param>
        public static void Show(this UIElement element) => element.Visibility = Visibility.Visible;

        /// <summary>
        /// display elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="show"></param>
        public static void Show(this UIElement element, bool show) => element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Unrealistic elements, but retain space
        /// </summary>
        /// <param name="element"></param>
        public static void Hide(this UIElement element) => element.Visibility = Visibility.Hidden;

        /// <summary>
        /// The element is not displayed and no space is reserved
        /// </summary>
        /// <param name="element"></param>
        public static void Collapse(this UIElement element) => element.Visibility = Visibility.Collapsed;
    }
}
