using Libraries.Custom.Icons.Enums;
using Libraries.Custom.Icons.Extensions;
using Libraries.Custom.Icons.Font.Images;
using Libraries.Custom.Icons.Helpers;

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
