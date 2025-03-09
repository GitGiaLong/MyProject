namespace Libraries.Custom.Icon.Materials
{
    public class IconImage : IconImageBase<MaterialIcons>
    {
        protected override ImageSource ImageSourceFor(MaterialIcons icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return MaterialDesignFont.Wpf.Value.ToImageSource(icon, Foreground, size);
        }
    }
}
