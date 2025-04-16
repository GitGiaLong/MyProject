using Libraries.Custom.Enums.Icon;
using Libraries.Custom.Helpers.Icon;
using Libraries.Custom.Icons.Extensions;
using Libraries.Custom.Icons.Font.Images;

namespace Libraries.Custom.Icons.Materials
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
