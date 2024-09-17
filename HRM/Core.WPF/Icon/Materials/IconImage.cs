using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Extensions;
using Core.WPF.Icon.Font.Images;
using Core.WPF.Icon.Helpers;

namespace Core.WPF.Icon.Materials
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
